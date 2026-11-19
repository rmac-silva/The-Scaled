using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheScaled.TheScaledCode.Powers;


public class NourishmentSetup : BaseSetupPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>(),HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        await PowerCmd.Apply<NourishmentPower>(ctx, Owner, Amount, Applier, null);
        await PowerCmd.Apply<NourishmentPower>(ctx, Applier!, Amount, Applier, null);
    }
}