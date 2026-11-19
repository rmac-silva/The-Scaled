using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheScaled.TheScaledCode.Powers;

public abstract class BaseSetupPower : TheScaledPower, ISetupPower
{
    public override PowerType Type => PowerType.None;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>()];

    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier;
    protected override bool IsVisibleInternal => LocalContext.IsMe(Applier);

    public Task TriggerSetup(PlayerChoiceContext ctx)
    {
        TriggerSetupWithNoReset(ctx);
        PowerCmd.Remove(this);
        return Task.CompletedTask;
    }

    public abstract Task TriggerSetupWithNoReset(PlayerChoiceContext ctx);
}