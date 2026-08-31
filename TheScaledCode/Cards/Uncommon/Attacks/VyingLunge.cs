using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheScaled.TheScaledCode.Cards;

  
public class VyingLunge : TheScaledCard
{
    public VyingLunge() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            return !_triggeredThisTurn;
        }
    }

    private bool _triggeredThisTurn = false;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.ForEnergy(base.Owner)];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new EnergyVar(1)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .WithHitCount(2)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        if(!_triggeredThisTurn)
        {
            await PlayerCmd.GainEnergy(1,base.Owner);
            _triggeredThisTurn = true;
        }
        
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if(side == CombatSide.Player)
        {
            _triggeredThisTurn = false;
        }
    }

}