using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;

  
public class MudBath : TheScaledCard
    {
        public MudBath() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
        {
        }
        
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new CardsVar(2)
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromAffliction<Muddied>().First(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(base.RunState,"RunState");
            List<CardModel>? validCards = CardPile.Get(PileType.Draw,base.Owner)?.Cards.Where(Muddied.CanAfflictMuddied).ToList();

            if(validCards is null)
            {
                return;
            }
            for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
            {
                if(validCards.Count() == 0)
                {
                    return;
                }

                CardModel? c = base.RunState.Rng.CombatCardSelection.NextItem(validCards);

                if(c is null)
                {
                    return;
                } else
                {
                    c.AfflictInternal(ModelDb.Affliction<Muddied>(),1);
                    await CardPileCmd.Add(c,PileType.Hand);
                }

                validCards.Remove(c);
            }
            
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Cards.UpgradeValueBy(1);
        }
    }

