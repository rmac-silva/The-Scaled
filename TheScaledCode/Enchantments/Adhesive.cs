using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Enchantments
{
    public class Adhesive : CustomEnchantmentModel
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new EnergyVar(1)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];
        

        public override bool IsStackable => false;

        protected override string? CustomIconPath => base.CustomIconPath;

        public override bool CanEnchantCardType(CardType cardType)
        {
            return cardType == CardType.Attack
                || cardType == CardType.Skill;
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

            if(card.Keywords.Contains(CardKeyword.Exhaust))
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
            await PlayerCmd.GainEnergy(-base.DynamicVars.Energy.BaseValue,base.Card.Owner);
        }

        public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources, CardLocation cardLocation)
    {
        //If it's not our card we ignore it
        if (card.Owner != base.Card.Owner)
		{
			return cardLocation;
		}

        //If it's not the enchanted card, we ignore it
        if(card != base.Card)
        {
            return cardLocation;
        }

        //If it's not being moved to the discard pile
        if (!CanBeReturned(cardLocation.pileType))
		{
			return cardLocation;
		}
        
        //Otherwise, place it back into your hand
        return new CardLocation(card.Owner,PileType.Hand,CardPilePosition.Top);
    }

    /// <summary>
    /// Returns true if the destination pile is one that can be affecte by Adhesive
    /// </summary>
    /// <param name="destinationPile"></param>
    /// <returns></returns>
    private bool CanBeReturned(PileType destinationPile)
        {
            return destinationPile == PileType.Draw || destinationPile == PileType.Discard;
        }


    protected override void OnEnchant()
        {
            base.Card.AddKeyword(CardKeyword.Retain);
        }
    }

}
