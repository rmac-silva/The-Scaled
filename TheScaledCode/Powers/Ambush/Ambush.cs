using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace TheScaled.TheScaledCode.Powers;

public struct AmbushMethodInfo
{
    public Creature target;
    public PlayerChoiceContext choiceContext;

    public AmbushMethodInfo(Creature target, PlayerChoiceContext choiceContext)
    {
        this.target = target;
        this.choiceContext = choiceContext;
    }
}

public delegate Task AmbushEffect(AmbushMethodInfo info);

public class Ambush : TheScaledPower
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier;
    protected override bool IsVisibleInternal => LocalContext.IsMe(base.Applier);
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("AmbushThreshold", 0m), new DynamicVar("AmbushThresholdBase", 5m)];
    


    /// <summary>
    /// Checks if the Ambush power has been applied to a creature, and logs the application.
    /// </summary>
    /// <param name="applier"></param>
    /// <param name="cardSource"></param>
    /// <returns></returns>
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        ResetAmbushThreshold();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Checks if the Ambush threshold has been reached, and if so, triggers the ambush.
    /// </summary>
    /// <param name="choiceContext"></param>
    /// <param name="power"></param>
    /// <param name="amount"></param>
    /// <param name="applier"></param>
    /// <param name="cardSource"></param>
    /// <returns></returns>
    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        if (power != this)
        {
            return;
        }
       
        if (base.Amount >= base.DynamicVars["AmbushThreshold"].IntValue)
        {
            await TriggerAmbush(
                new AmbushMethodInfo(
                    target: Owner,
                    choiceContext: choiceContext
                )
            );
        }

        if (base.Amount < 0)
        {
            base.SetAmount(0);
        }
        return;
    }

    /// <summary>
    /// Triggers the ambush, resetting the count back to 1 and triggering all
    /// Setup effects
    /// </summary>
    private async Task TriggerAmbush(AmbushMethodInfo info)
    {
        //Fetch powers for creature that implement ISetupPower, and trigger their setup methods
        var setupPowers = base.Owner.Powers.Where(p => p is ISetupPower && p.Applier == Applier).Cast<ISetupPower>().ToList();
        //Create Cloud VFX
        if(setupPowers.Count() != 0)
        {
            var vfx = NSetupImpactVfx.Create(info.target);
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(vfx);
            if(vfx != null) { _ = vfx.ShowClouds(setupPowers.Count()); }
        }
        
        SetAmount(0);
        ResetAmbushThreshold();
        bool shouldResetEffects = ShouldResetEffects();

        // Do the registered effects last, since they can re-add setups & such
        foreach (var entry in setupPowers)
        {
            if(shouldResetEffects)
            {
                await entry.TriggerSetup(info.choiceContext);
            }
            else
            {
                await entry.TriggerSetupWithNoReset(info.choiceContext);
            }
        }

        ClearResetEffects();

        EventManager.InvokeAmbushTriggered(base.Applier!.Player!);
    }

    /// <summary>
    /// Public method for triggering an Ambush from external sources. Could be from a relic.
    /// Ignores threshold amount and triggers an ambush immediately
    /// </summary>
    /// <param name="choiceContext"></param>
    /// <returns></returns>
    public async Task TriggerAmbushExternal(PlayerChoiceContext? choiceContext = null)
    {
        await TriggerAmbush(
            new AmbushMethodInfo(
                target: Owner,
                choiceContext: choiceContext ?? new ThrowingPlayerChoiceContext()
            )
        );
    }

    

    /// <summary>
    /// Changes the base Ambush threshold externally.
    /// </summary>
    /// <param name="newAmount"></param>
    public void ChangeAmbushThreshold(int newAmount)
    {
        base.DynamicVars["AmbushThresholdBase"].BaseValue = newAmount;
        ResetAmbushThreshold();
    }

    /// <summary>
    /// Resets the Ambush threshold back to the base value.
    /// </summary>
    private void ResetAmbushThreshold()
    {
        base.DynamicVars["AmbushThreshold"].BaseValue =
            base.DynamicVars["AmbushThresholdBase"].BaseValue;
    }

    private bool ShouldResetEffects()
    {
        var ignoreRefreshPower = Owner.Powers.FirstOrDefault(p => p is KeepSetupsPower && p.Applier == base.Applier);
        return ignoreRefreshPower is null;
    }

    private void ClearResetEffects()
    {
        PowerCmd.Remove<KeepSetupsPower>(Owner);
    }

}
