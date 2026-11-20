using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheScaled.TheScaledCode.Powers;




  
public class BlockSetup : BaseSetupPower
{
    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        await CreatureCmd.GainBlock(Applier!, Amount, ValueProp.Unpowered, null);
    }
}