using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;

  
public class TailSweep : TheScaledCard
{
    public TailSweep() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),new RepeatVar(2), new DynamicVar("MuddiedCount",2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromAffliction<Muddied>().First()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        ArgumentNullException.ThrowIfNull(CombatState, "CombatState");

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .TargetingAllOpponents(base.CombatState)
            .WithHitCount(base.DynamicVars.Repeat.IntValue)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        

        var pile = PileType.Discard.GetPile(base.Owner);
        await Mud.MuddyCards(pile,base.DynamicVars["MuddiedCount"].IntValue,base.Owner);

    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3);
    }
}