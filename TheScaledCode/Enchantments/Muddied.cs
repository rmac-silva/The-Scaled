using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Enchantments
{
    public class Muddied : CustomEnchantmentModel
    {

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

            //Pick a random card from the draw pile
            var pile = CardPile.Get(PileType.Draw, base.Card.Owner);

            if(pile == null || pile.Cards.Count == 0)
            {
                return;
            }

            CardModel? cardModel = Card.Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
            if (cardModel != null)
            {
                CardCmd.Enchant<Muddied>(cardModel, 1);
                CardCmd.Preview(cardModel,0.3f);
                await CardPileCmd.Add(cardModel, PileType.Discard, CardPilePosition.Top);

            }
        }
    }
}
