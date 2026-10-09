using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Afflictions;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
    public class Camouflage : TheScaledCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new CalculationBaseVar(0),
                new CalculationExtraVar(2),
                new CalculatedBlockVar(MegaCrit.Sts2.Core.ValueProps.ValueProp.Move).WithMultiplier(
                    (c, _) => GetNumMuddiedCardsInHand(c.Owner)
                ),
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromAffliction<Muddied>().First()];

        public Camouflage()
            : base(1, CardType.Skill, CardRarity.Common, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(
                Owner.Creature,
                base.DynamicVars.CalculatedBlock.IntValue,
                MegaCrit.Sts2.Core.ValueProps.ValueProp.Move,
                cardPlay
            );
        }

        private static int GetNumMuddiedCardsInHand(Player owner)
        {
            List<CardModel>? cards = CardPile
                .Get(PileType.Hand, owner)
                ?.Cards.Where((CardModel c) => c.Affliction != null && c.Affliction is Muddied)
                .ToList();

            if (cards is not null)
            {
                return cards.Count;
            }

            return 0;
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(1);
        }
    }
}
