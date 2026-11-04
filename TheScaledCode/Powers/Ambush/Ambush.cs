using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;

/// <summary>
/// Struct for storing information to be used by Setup cards. Can contain a variety of information
/// to use during the ambush trigger.
/// </summary>
public struct AmbushEntry
{
    public AmbushEffect effect;
    public SetupCard source;

    public AmbushEntry(AmbushEffect e, SetupCard src)
    {
        effect = e;
        source = src;
    }
}

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
    public override LocString Title => getFormattedTitle();
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override bool IsVisibleInternal => true; //Not visible on enemies in the future.
    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier; //One per player, stacking
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("AmbushThreshold", 0m), new DynamicVar("AmbushThresholdBase", 5m)];
    private List<AmbushEntry> EffectsForOwner => _queuedEffects;

    private List<AmbushEntry> _queuedEffects = [];
    
    

    public int NumSetupsForCreature
    {
        get
        {
            return _queuedEffects.Count;
        }
    }

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
        //Create Cloud VFX
        if(EffectsForOwner.Count != 0)
        {
            var vfx = NSetupImpactVfx.Create(info.target);
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(vfx);
            if(vfx != null) { _ = vfx.ShowClouds(NumSetupsForCreature); }
        }
        

        var effects = EffectsForOwner;
        var savedListOfEffects = effects.ToArray(); //Save the list of effects to a temporary copy, to avoid modifying the existing list

        if(ShouldResetEffects()) //Reset the effects first
        {
            effects.Clear();
            GetSetupPowerForApplier()?.RemoveSetup();
        }

        base.SetAmount(0);
        ResetAmbushThreshold();

        // Do the registered effects last, since they can re-add setups & such
        foreach (var entry in savedListOfEffects)
        {
            await entry.effect(info);
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
    /// Adds an ambush effect to the queue.
    /// This is called by Setup cards to register their effects.
    /// </summary>
    /// <param name="effect"></param>
    public async Task AddAmbushEffect(AmbushEntry effect)
    {
        if (base.Owner is null || base.Applier is null)
        {
            ModLog.Warning(
                this,
                $"Cannot add ambush effect for {effect.source.Title}: owner or applier is null. Owner={base.Owner}, Applier={base.Applier}"
            );
            return;
        }

        var setupPower = GetSetupPowerForApplier();

        if (setupPower is null)
        {
            setupPower = await PowerCmd.Apply<SetupPower>(
                new ThrowingPlayerChoiceContext(),
                base.Owner,
                1,
                base.Applier,
                null
            );
        }

        if(setupPower is null || setupPower.Owner is null) //Artifact
        {
            return;
        }
        setupPower.AddHovertip(effect.source.GetHovertip);

        EffectsForOwner.Add(effect);
    }

    /// <summary>
    /// Adds a pre-existing ambush effect. This means that the hovertip already exists, and we simply need to add it to the list of effects.
    /// </summary>
    /// <param name="effect"></param>
    public async Task AddExistingAmbushEffect(AmbushEntry effect)
    {
        var setupPower = GetSetupPowerForApplier();

        if (setupPower is null)
        {
            setupPower = await PowerCmd.Apply<SetupPower>(
                new ThrowingPlayerChoiceContext(),
                base.Owner,
                1,
                base.Applier,
                null
            );
        }

        if(setupPower is null || setupPower.Owner is null)//Artifact
        {
            return;
        }

        await AddAmbushEffect(effect);
    }

    /// <summary>
    /// Randomly copies and applies a single Setup that's already applied.
    /// </summary>
    /// <returns></returns>
    public async Task CopyAndApplyRandomAmbushEffect(IRunState runState)
    {
        //Fetch existing effects
        if (_queuedEffects.Count == 0)
        {
            return;
        }

        //Pick a random one
        var chosenEffect = runState.Rng.Niche.NextItem(_queuedEffects);

        await AddExistingAmbushEffect(chosenEffect);

        CardCmd.Preview(chosenEffect.source, 0.8f);
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

    /// <summary>
    /// Fetches the Setup power for this enemy, applied by our player
    /// </summary>
    /// <returns></returns>
    private SetupPower? GetSetupPowerForApplier()
    {
        try
        {
            return Owner.Powers
                .OfType<SetupPower>()
                .FirstOrDefault(power => power.Applier == base.Applier);
        }
        catch (Exception)
        {
            return null;
        }
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

    protected override void AfterCloned()
    {
        base.AfterCloned();
        _queuedEffects = [];
    }

}
