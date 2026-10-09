using BaseLib.Abstracts;
using Godot;
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

        private const int MUD_CARDS = 2;
        

        public override bool CanAfflictCardType(CardType card)
        {
            return true;
        }

        public override bool CanAfflict(CardModel card)
        {
            return CanAfflictMuddied(card);
        }

        public override async Task OnPlay(PlayerChoiceContext choiceContext, Creature? target)
        {
            await Mud.AddMudCard(PileType.Draw,MUD_CARDS, Card.Owner);
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

            if(card.Affliction != null)
            {
                //Override the affliction with Muddied
                card.ClearAfflictionInternal();
            }

            return true;
        }
    }
}
