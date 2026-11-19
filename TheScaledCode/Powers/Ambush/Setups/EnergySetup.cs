using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheScaled.TheScaledCode.Powers;


public class EnergySetup : BaseSetupPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>(),HoverTipFactory.ForEnergy(this)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        if (Applier == null || !Applier.IsPlayer) return;
        await PlayerCmd.GainEnergy(Amount, Applier.Player!);
    }
}