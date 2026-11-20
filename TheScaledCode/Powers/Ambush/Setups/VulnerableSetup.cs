using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheScaled.TheScaledCode.Powers;




public class VulnerableSetup : BaseSetupPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>(),HoverTipFactory.FromPower<VulnerablePower>()];

    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        await PowerCmd.Apply<VulnerablePower>(ctx, Owner, Amount, Applier, null);
    }
}