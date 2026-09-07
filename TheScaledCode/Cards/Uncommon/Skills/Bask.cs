using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
    public class Bask : TheScaledCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new BlockVar(8, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
                new CardsVar(2),
                new DynamicVar("BurnAmount", 1),
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<Ambush>()];

        public Bask()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            ArgumentNullException.ThrowIfNull(
                base.Owner.Creature.CombatState,
                "Owner.Creature.CombatState"
            );

            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
            await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);

            List<CardModel> list = new List<CardModel>();
            for (int i = 0; i < base.DynamicVars["BurnAmount"].IntValue; i++)
            {
                CardModel card = base.Owner.Creature.CombatState.CreateCard<Mud>(base.Owner);
                list.Add(card);
            }
            //Add the cards to the discard pile
            await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, base.Owner);
            CardCmd.Preview(list, 0.4f);

            await base.OnPlay(choiceContext, cardPlay);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(3);
        }
    }
}
