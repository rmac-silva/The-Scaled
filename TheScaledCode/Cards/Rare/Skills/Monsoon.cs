using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Cards
{

  
  
  
  
public class Monsoon : TheScaledCard
    {
        public Monsoon() : base(0, CardType.Skill, CardRarity.Rare, TargetType.None)
        {
        }

        
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
           foreach (CardModel item in PileType.Exhaust.GetPile(base.Owner).Cards.ToList())
            {
                await CardPileCmd.Add(item, PileType.Draw,CardPilePosition.Random);
                if(IsUpgraded && item.IsUpgradable)
                {
                    CardCmd.Upgrade(item);
                }
            }
        }

        protected override void OnUpgrade()
        {
        }
    }
}
