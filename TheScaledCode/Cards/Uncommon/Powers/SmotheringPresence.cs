using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers.Cards;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;


public class SmotheringPresence : TheScaledCard
{
    public SmotheringPresence() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<DrownedPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("PowerAmount", 1),
        new DynamicVar("ExtraConsumption",2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        (await PowerCmd.Apply<SmotheringPresencePower>(choiceContext, base.Owner.Creature, base.DynamicVars["PlatingPower"].IntValue, base.Owner.Creature, this))?.AddDrownConsumption(base.DynamicVars["ExtraConsumption"].IntValue);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["ExtraConsumption"].UpgradeValueBy(-1);
    }
}