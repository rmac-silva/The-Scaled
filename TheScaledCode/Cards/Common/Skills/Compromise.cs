using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Enchantments;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
      
  
  
public class Compromise : TheScaledCard
    {
        
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("ExertionDuration",3), new EnergyVar(2)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [base.EnergyHoverTip,HoverTipFactory.FromPower<ExertionPower>(),HoverTipFactory.FromEnchantment<Muddied>().First()];

        public Compromise() : base(1, CardType.Skill, CardRarity.Common, TargetType.None)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            //Gain 2 energy
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
            
            //Gain 1 exertion for two(three) turns
            await PowerCmd.Apply<CompromisePower>(choiceContext,base.Owner.Creature,DynamicVars["ExertionDuration"].BaseValue,base.Owner.Creature,cardPlay.Card);

            //Enchant a random card with muddied
            CardPile pile = PileType.Hand.GetPile(base.Owner);
            CardModel? cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
            if (cardModel != null)
            {
                CardCmd.Enchant<Muddied>(cardModel,1);
            }
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
            base.DynamicVars["ExertionDuration"].UpgradeValueBy(-1);
        }
    }
}