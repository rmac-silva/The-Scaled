using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{

  
  
  
  
public class Rinse : TheScaledCard
    {
        public Rinse() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
        {
        }

        protected override bool ShouldGlowGoldInternal
        {
        get
        {
            return CardPile.Get(PileType.Draw, Owner)?.Cards.Any((CardModel c) => c.Affliction != null) ?? false;
        }
    }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new CardsVar(3),
                new BlockVar(8,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<DrownedPower>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];



        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(base.RunState,"RunState");

            await CreatureCmd.GainBlock(base.Owner.Creature,base.DynamicVars.Block,cardPlay);

            var cards = CardPile.Get(PileType.Draw, Owner)?.Cards.Where((CardModel c) => c.Affliction != null).ToList();
            var cardsAffected = new List<CardModel>(base.DynamicVars.Cards.IntValue);
            if(cards is null)
            {
                return;
            }

            for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
            {
                if(cards.Count() == 0)
                {
                    return;
                }

                var cardToRemofeAff = base.RunState.Rng.CombatCardSelection.NextItem(cards);

                if(cardToRemofeAff is null)
                {
                    continue;
                }

                cardsAffected.Add(cardToRemofeAff);
                cardToRemofeAff.ClearAfflictionInternal();
                cards.Remove(cardToRemofeAff);
            }

            CardCmd.Preview(cardsAffected,0.8f);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(3);
        }
    }
}
