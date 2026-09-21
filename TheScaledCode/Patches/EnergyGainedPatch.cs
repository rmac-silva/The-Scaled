
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Runs;
using TheScaled.TheScaledCode.Extensions;

/// <summary>
/// Logs the energy reset
/// </summary>
[HarmonyPatch(typeof(PlayerCombatState), nameof(PlayerCombatState.ResetEnergy))]
public static class MaxEnergyResetPatch
{
    
    public static void Postfix(PlayerCombatState __instance)
    {
        Player? player = AccessTools.Field(typeof(PlayerCombatState), "_player").GetValue(__instance) as Player;
        if (player?.Creature?.CombatState is not { } combatState)
        {
            return;
        }
        ModLog.Info(__instance,$"Creating new energy gained entry: {__instance.MaxEnergy}");
        CombatManager.Instance.History.EnergyGained(combatState, __instance.MaxEnergy, player.Creature);
    }
}

/// <summary>
/// Logs the energy reset when the player has ice cream
/// </summary>
[HarmonyPatch(typeof(PlayerCombatState), nameof(PlayerCombatState.AddMaxEnergyToCurrent))]
public static class MaxEnergyIncrementPatch
{
    
    public static void Postfix(PlayerCombatState __instance)
    {
        Player? player = AccessTools.Field(typeof(PlayerCombatState), "_player").GetValue(__instance) as Player;
        if (player?.Creature?.CombatState is not { } combatState)
        {
            return;
        }
        ModLog.Info(__instance,$"Creating new energy gained entry: {__instance.MaxEnergy}");
        CombatManager.Instance.History.EnergyGained(combatState, __instance.MaxEnergy, player.Creature);
    }
}

/// <summary>
/// Logs energy gains from other sources
/// </summary>
[HarmonyPatch(typeof(PlayerCombatState), nameof(PlayerCombatState.GainEnergy))]
public static class GainEnergyFromOtherSourcesPatch
{
    
    public static void Postfix(PlayerCombatState __instance, decimal amount)
    {
        Player? player = AccessTools.Field(typeof(PlayerCombatState), "_player").GetValue(__instance) as Player;
        if (player?.Creature?.CombatState is not { } combatState)
        {
            return;
        }

        if(amount < 0)
        {
            return;
        }
        ModLog.Info(__instance,$"Creating new energy gained entry: {amount}");
        CombatManager.Instance.History.EnergyGained(combatState, (int)amount, player.Creature);
    }
}
