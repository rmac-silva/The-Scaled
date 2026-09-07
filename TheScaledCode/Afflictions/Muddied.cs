using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Afflictions
{
    public class Muddied : AfflictionModel, ICustomModel
    {

        public override bool IsStackable => true;
        public override bool CanAfflictUnplayableCards => false;
        public override bool HasExtraCardText => true;
        private bool CanEnchantCardType(CardType cardType)
        {
            return cardType == CardType.Attack
                || cardType == CardType.Skill
                || cardType == CardType.Power;
        }

        public override bool CanAfflictCardType(CardType card)
        {
            // Check card type validation
            if (!CanEnchantCardType(card))
            {
                return false;
            }
            return true;
        }

        public override bool CanAfflict(CardModel card)
	{
		if (!CanAfflictCardType(card.Type))
		{
			return false;
		}
		if (card.Keywords.Contains(CardKeyword.Unplayable) && !CanAfflictUnplayableCards)
		{
			return false;
		}
		if (card.Affliction != null && (!IsStackable || card.Affliction.GetType() != GetType()))
		{
			return false;
		}
		return true;
	}

        public override async Task OnPlay(PlayerChoiceContext choiceContext, Creature? target)
        {
            ModLog.Info(this,$"Muddied Resource Exists: {HasOverlay} | Checked Path: {OverlayPath}. | Backup: {ResourceLoader.Exists(OverlayPath)}");
            //Pick a random card from the draw pile
            var pile = CardPile.Get(PileType.Draw, base.Card.Owner);

            if(pile == null || pile.Cards.Count == 0)
            {
                return;
            }

            CardModel? cardModel = Card.Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
            if (cardModel != null)
            {
                await CardCmd.Afflict<Muddied>(cardModel, 1);
                await CardPileCmd.Add(cardModel, PileType.Discard, CardPilePosition.Top);
                CardCmd.Preview(cardModel,0.8f);

            }
        }
    }
}
