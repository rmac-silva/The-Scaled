using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Powers;

[HarmonyPatch(typeof(PowerModel), nameof(PowerModel.ShouldRemoveDueToAmount))]
public static class ShouldRemoveDueToAmountPatch
{
    private static readonly HashSet<Type> IgnoredPowerTypes = new()
    {
        typeof(ExertionPower),
        typeof(TemporaryExertionDownPower)
    };

    /// <summary>
    /// This patch checks if the power that now has '0' Amount is one of the exceptions. 
    /// If so, it will avoid removing the power when it reaches an amount of 0.
    /// </summary>
    /// <param name="__instance"></param>
    /// <param name="__result"></param>
    /// <returns></returns>
    public static bool Prefix(PowerModel __instance, ref bool __result)
    {
        if (IgnoredPowerTypes.Contains(__instance.GetType()))
        {
            if (__instance.Amount <= 0)
            {
                __result = false; // Do NOT remove
                return false;    // Skip original method
            }
        }

        // 2. Let the game handle all other vanilla powers normally
        return true; 
    }
}