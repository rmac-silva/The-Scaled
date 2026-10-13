// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.HoverTips;
// using MegaCrit.Sts2.Core.Models;
// using TheScaled.TheScaledCode.Afflictions;

// namespace TheScaled.TheScaledCode.Cards
// {

  
  
  
  
  
// public class DiveBomb : TheScaledCard
//     {
//         public DiveBomb() : base(1, CardType.Skill, CardRarity.Rare, TargetType.None)
//         {
//         }

//         protected override IEnumerable<IHoverTip> ExtraHoverTips =>
//             [HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromKeyword(CardKeyword.Innate)];
//         public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,CardKeyword.Innate];


//         protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//         {
//            var cards = CardPile.Get(PileType.Hand, Owner)!.Cards;

//            if(cards is null || cards.Count == 0)
//            {
//                 return;
//            }

//            foreach(CardModel c in cards)
//             {
//                 await CardCmd.Afflict<Muddied>(c, 1);
//             }


//         }

//         protected override void OnUpgrade()
//         {
//             base.EnergyCost.UpgradeBy(-1);
//         }
//     }
// }
