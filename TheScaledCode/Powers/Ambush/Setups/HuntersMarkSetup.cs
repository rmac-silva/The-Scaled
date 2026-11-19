using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheScaled.TheScaledCode.Powers;

public class HuntersMarkSetup : BaseSetupPower
{
    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        await PowerCmd.Apply<HuntersMarkPower>(ctx, Owner, Amount, Applier, null);
    }
}