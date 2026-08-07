using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Enchantments;
using TheScaled.TheScaledCode.Helpers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
      
  

public class ToughenUp : TheScaledCard
    {
        public override bool GainsBlock => true;

        //! If you change the values here, you have to change them in the ToughenUpPower class as well!
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(5m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("DexterityUp",1), new PowerVar<ThornsPower>(2)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DexterityPower>(), HoverTipFactory.FromPower<ThornsPower>(), HoverTipFactory.FromEnchantment<Muddied>().First()];

        public ToughenUp() : base(0, CardType.Skill, CardRarity.Common, TargetType.None)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(base.Owner.Creature, DynamicVars.Block, cardPlay);

            await PowerCmd.Apply<ToughenUpPower>(choiceContext, base.Owner.Creature, 1, base.Owner.Creature, cardPlay.Card);
            
            //Enchant a random card with muddied
            CardPile pile = PileType.Hand.GetPile(base.Owner);
            IEnumerable<CardModel> validCards = pile.Cards.Where(EnchanteableHelper.CanBeEnchantedByMuddied);

            if(validCards.Count() == 0)
            {
                return;
            }

            CardModel? cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem(validCards);
            if (cardModel != null)
            {
                CardCmd.Enchant<Muddied>(cardModel,1);
            }
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars["DexterityUp"].UpgradeValueBy(1);
            base.DynamicVars["ThornsPower"].UpgradeValueBy(1);
        }
    }
}