using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards
{
    public class Camouflage : TheScaledCard
    {

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new CalculationBaseVar(0),
                new CalculationExtraVar(1),
                new CalculatedBlockVar(MegaCrit.Sts2.Core.ValueProps.ValueProp.Move).WithMultiplier(
                    (c, _) => GetNumMuddiedCardsInDeck(c.Owner)
                ),
            ];

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                if (base.CombatState == null)
                {
                    return false;
                }
                return CardPile.Get(PileType.Hand, Owner)?.Cards.Any((CardModel c) => c.Affliction != null && c.Affliction is Muddied) ?? false;
            }
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromAffliction<Muddied>().First()];

        public Camouflage()
            : base(1, CardType.Skill, CardRarity.Common, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), DynamicVars.CalculatedBlock.Props, cardPlay);
        }

        private static int GetNumMuddiedCardsInDeck(Player owner)
        {
            List<CardModel>? handCards = CardPile
                .Get(PileType.Hand, owner)
                ?.Cards.Where((CardModel c) => c.Affliction != null && c.Affliction is Muddied)
                .ToList();
            List<CardModel>? drawCards = CardPile
                .Get(PileType.Draw, owner)
                ?.Cards.Where((CardModel c) => c.Affliction != null && c.Affliction is Muddied)
                .ToList();
            List<CardModel>? discardCards = CardPile
                .Get(PileType.Discard, owner)
                ?.Cards.Where((CardModel c) => c.Affliction != null && c.Affliction is Muddied)
                .ToList();

            return (handCards?.Count ?? 0) + (drawCards?.Count ?? 0) + (discardCards?.Count ?? 0);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.CalculationExtra.UpgradeValueBy(1);
        }
    }
}
