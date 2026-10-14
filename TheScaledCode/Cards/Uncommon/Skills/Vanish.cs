using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class Vanish : TheScaledCard
{
    public Vanish()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    protected override bool ShouldGlowGoldInternal => base.CombatState?.HittableEnemies.Any((Creature e) => e.HasPower<DrownedPower>()) ?? false;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<DrownedPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0m),
		new CalculationExtraVar(1m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel _, Creature? c) => c != null ? c.GetPowerAmount<DrownedPower>() : 0)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), base.DynamicVars.CalculatedBlock.Props, cardPlay);
        await PowerCmd.Remove<DrownedPower>(cardPlay.Target!);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.CalculationExtra.UpgradeValueBy(1);
    }

}
