using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheScaled.TheScaledCode.Powers;
using TheScaled.TheScaledCode.Powers.Cards;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;


public class Dwelling : TheScaledCard
{
    public Dwelling() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<DwellingPower>(choiceContext, base.Owner.Creature, 1, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}