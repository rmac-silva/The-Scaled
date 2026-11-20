using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheScaled.TheScaledCode.Powers;


public class WeakSetup : BaseSetupPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>(),HoverTipFactory.FromPower<WeakPower>()];

    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        await PowerCmd.Apply<WeakPower>(ctx, Owner, Amount, Applier, null);
    }
}