using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Afflictions;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{

  
  
  
  
  
public class MudBath : TheScaledCard
    {
        public MudBath() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.None)
        {
        }

        

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new CardsVar(4),
                new DynamicVar("AfflictedCards",2)
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromAffliction<Muddied>().First(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {

            var pile = CardPile.Get(PileType.Draw,base.Owner);
            var handPile = CardPile.Get(PileType.Hand,base.Owner);

            if(pile is null || handPile is null)
            {
                return;
            }

            var cardsAffected = await Mud.MuddyCards(pile,base.DynamicVars["AfflictedCards"].IntValue,base.Owner);

            foreach(var card in cardsAffected)
            {

                await CardPileCmd.Add(card, handPile);
            }
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars["AfflictedCards"].UpgradeValueBy(1);
        }
    }
}
