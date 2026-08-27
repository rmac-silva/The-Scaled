using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
namespace TheScaled.TheScaledCode.Powers;

/// <summary>
/// Struct for storing information to be used by Setup cards. Can contain a variety of information
/// to use during the ambush trigger.
/// </summary>
public struct AmbushMethodInfo
{
    public Creature? applier;
    public Creature? target;
    public PlayerChoiceContext? choiceContext;

    public AmbushMethodInfo(Creature? applier = null, Creature? target = null, PlayerChoiceContext? choiceContext = null)
    {
        this.applier = applier;
        this.target = target;
        this.choiceContext = choiceContext;
    }
}

public delegate Task AmbushEffect(AmbushMethodInfo info);
  
  
public class Ambush : TheScaledPower
{
    
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => false;
    protected override bool IsVisibleInternal => true; //Not visible on enemies in the future.
    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier; //One per player, stacking
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("AmbushThreshold",0m),
        new DynamicVar("AmbushThresholdBase",5m),
    ];
    private List<AmbushEffect> EffectsForOwner =>
    _queuedEffects.TryGetValue(base.Owner, out var effects)
        ? effects
        : (_queuedEffects[base.Owner] = []);

    private readonly Dictionary<Creature, List<AmbushEffect>> _queuedEffects = [];

    /// <summary>
    /// Checks if the Ambush power has been applied to a creature, and logs the application.
    /// </summary>
    /// <param name="applier"></param>
    /// <param name="cardSource"></param>
    /// <returns></returns>
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        ModLog.Info(this,$"Ambush has been applied to {base.Owner}");
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
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if(power != this)
        {
            return;
        }
        ModLog.Info(this,$"Ambush has been changed to {base.Amount} | {amount} | Threshold: {base.DynamicVars["AmbushThreshold"].IntValue}");
        if(base.Amount >= base.DynamicVars["AmbushThreshold"].IntValue)
        {
            await TriggerAmbush(new AmbushMethodInfo(applier: base.Applier, target:base.Owner, choiceContext: choiceContext));
        }

        if(base.Amount < 0)
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
        var effects = EffectsForOwner;

        foreach (var effect in effects.ToArray())
        {
            await effect(info);
        }

        effects.Clear();
        base.Owner.GetPower<SetupPower>()?.RemoveSetup();
        base.SetAmount(0);
        ResetAmbushThreshold();
    }

    /// <summary>
    /// Public method for triggering an Ambush from external sources. Could be from a relic.
    /// Ignores threshold amount and triggers an ambush immediately 
    /// </summary>
    public async Task TriggerAmbushExternal()
    {
        await TriggerAmbush(new AmbushMethodInfo(applier:base.Applier, target :base.Owner, choiceContext: new ThrowingPlayerChoiceContext()));
    }

    /// <summary>
    /// Adds an ambush effect to the queue. 
    /// This is called by Setup cards to register their effects.
    /// </summary>
    /// <param name="effect"></param>
    public async Task AddAmbushEffect(AmbushEffect effect, HoverTip hoverTip)
    {
        var setupPower = base.Owner.GetPower<SetupPower>();

        if (setupPower is null)
        {
            setupPower = await PowerCmd.Apply<SetupPower>(
                new ThrowingPlayerChoiceContext(),
                base.Owner,
                1,
                null,
                null);
        }

        //Increase the threshold by one, representing the added setup effect
        base.DynamicVars["AmbushThreshold"].BaseValue++;

        setupPower?.AddHovertip(hoverTip);
        EffectsForOwner.Add(effect);
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
        base.DynamicVars["AmbushThreshold"].BaseValue = base.DynamicVars["AmbushThresholdBase"].BaseValue + GetNumSetups();
        
    }

    private int GetNumSetups()
    {
        return base.Owner.GetPower<SetupPower>()?.AmbushThresholdIncrease ?? 0;
    }
    
}