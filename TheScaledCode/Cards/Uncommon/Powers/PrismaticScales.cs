using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class PrismaticScales : TheScaledCard
{
    public PrismaticScales() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DrownAmount",1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<PrismaticScalesPower>(choiceContext,base.Owner.Creature,base.DynamicVars["DrownAmount"].IntValue,base.Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["DrownAmount"].UpgradeValueBy(1);
    }
}