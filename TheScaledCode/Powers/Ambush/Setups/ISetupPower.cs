using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheScaled.TheScaledCode.Powers;

/// <summary>
/// Defines an interface for powers that can be used in conjunction with Setup cards. This interface defines the minimum methods 
/// </summary>
public interface ISetupPower
{
    public Task TriggerSetup(PlayerChoiceContext info);
    public Task TriggerSetupWithNoReset(PlayerChoiceContext info);
}