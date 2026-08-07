using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Helpers
{
    public static class EnchanteableHelper
    {
        public static bool CanBeEnchantedByMuddied(CardModel t)
        {
            return t.Type != CardType.Status && t.Type != CardType.Curse && t.Type != CardType.Quest && t.Enchantment is null;
        }
    }
}
