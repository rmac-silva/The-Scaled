using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Enchantments
{
    public class Muddied : CustomEnchantmentModel
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DynamicVar("DexterityDown", 1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<MudPower>()];

        public override bool IsStackable => true;

        protected override string? CustomIconPath => base.CustomIconPath;

        public override bool CanEnchantCardType(CardType cardType)
        {
            return cardType == CardType.Attack
                || cardType == CardType.Skill
                || cardType == CardType.Power;
        }

        public override bool CanEnchant(CardModel card)
        {
            if (card == null)
                return false;

            // Check card type validation
            if (!CanEnchantCardType(card.Type))
            {
                return false;
            }

            // Check deck unplayable status
            CardPile? pile = card.Pile;
            if (
                pile != null
                && pile.Type == PileType.Deck
                && card.Keywords.Contains(CardKeyword.Unplayable)
            )
            {
                return false;
            }

            // Check existing enchantments
            if (
                card.Enchantment != null
                && (!IsStackable || card.Enchantment.GetType() != GetType())
            )
            {
                return false;
            }

            return true;
        }

        public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
        {
            await PowerCmd.Apply<MudPower>(
                choiceContext,
                base.Card.Owner.Creature,
                base.DynamicVars["DexterityDown"].BaseValue,
                base.Card.Owner.Creature,
                base.Card
            );
        }
    }
}
