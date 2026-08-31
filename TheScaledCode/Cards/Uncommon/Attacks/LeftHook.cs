using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheScaled.TheScaledCode.Cards;

 
public class LeftHook : TheScaledCard
{
    public LeftHook() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    private bool _triggeredThisTurn = false;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
        new DynamicVar("DamageThreshold", 20)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd
        .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2);
        base.DynamicVars["DamageThreshold"].UpgradeValueBy(-5);
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {

        if(_triggeredThisTurn)
        {
            return;
        }

        if(dealer != null && dealer != base.Owner.Creature)
        {
            return;
        }

        //If it's not card damage, or the unblocked damage is lower than our threshold, we ignore it
        if(props != ValueProp.Move || result.UnblockedDamage < base.DynamicVars["DamageThreshold"].IntValue)
        {
            return;
        }

        //Return this to your hand
        await CardPileCmd.Add(this, PileType.Hand);
        
        //Register as retrieved
        _triggeredThisTurn = true;
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if(side == CombatSide.Player)
        {
            _triggeredThisTurn = false;
        }
    }
}