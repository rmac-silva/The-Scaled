using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheScaled.TheScaledCode.Powers;


public class DamageSetup : BaseSetupPower
{
    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        await CreatureCmd.Damage(ctx, Owner, Amount, ValueProp.Unpowered, Applier!);
    }
}