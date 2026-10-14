using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

public static class CardCostHelper
{
    public static void RemoveRelativeCostReductions(CardModel card, int valueToRemove)
    {
        List<LocalCostModifier> modifiers = Traverse.Create(card)
            .Field("_localModifiers")
            .GetValue<List<LocalCostModifier>>();
        
        var modToRemove = modifiers.FirstOrDefault(m => m.Type == LocalCostType.Relative && m.Amount == valueToRemove);
        if(modToRemove != null)
        {
            modifiers.Remove(modToRemove);
        }
    }
}