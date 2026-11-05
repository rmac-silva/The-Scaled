using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Afflictions
{
    public class Muddied : AfflictionModel, ICustomModel
    {

        public override bool IsStackable => true;
        public override bool CanAfflictUnplayableCards => false;
        public override bool HasExtraCardText => true;
        private const int MUD_CARDS = 1;
        

        public override bool CanAfflictCardType(CardType card)
        {
            return true;
        }

        public override Task AfterCardEnteredCombat(CardModel card)
        {
            if(card == base.Card && !card.Keywords.Contains(CardKeyword.Retain))
            {
                card.AddKeyword(CardKeyword.Retain);
            }
            return Task.CompletedTask;
        }


        public override bool CanAfflict(CardModel card)
        {
            return CanAfflictMuddied(card);
        }


        public override async Task OnPlay(PlayerChoiceContext choiceContext, Creature? target)
        {
            await Mud.AddMudCard(PileType.Discard, 1, Card.Owner);
            await Mud.AddMudCard(PileType.Draw, 1, Card.Owner);
        }

        public static bool CanAfflictMuddied(CardModel card)
        {
            if (card.Keywords.Contains(CardKeyword.Unplayable))
            {
                return false;
            }
            //Can't muddy muddied cards
            if (card.Affliction != null && card.Affliction is Muddied)
            {
                return false;
            }

            if(card is Mud)
            {
                return false;
            }

            if(card.Affliction != null)
            {
                //Override the affliction with Muddied
                CardCmd.ClearAffliction(card);
            }

            return true;
        }

        public static async Task<CardModel> AfflictMuddied(CardModel card)
        {
            if (!CanAfflictMuddied(card))
            {
                throw new InvalidOperationException($"Cannot afflict card {card.Title} with Muddied.");
            }

            await CardCmd.Afflict<Muddied>(card,1);
            card.AddKeyword(CardKeyword.Retain);
            return card;
        }
    }
}
