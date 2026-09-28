using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards
{

  
  
  
public class MAD : TheScaledCard
    {
        public MAD() : base(0, CardType.Skill, CardRarity.Rare, TargetType.None)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new EnergyVar(3),
                new CardsVar(2),
                new PowerVar<DrownedPower>(5),
            ];
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<DrownedPower>(), HoverTipFactory.ForEnergy(this), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
           //Gain 3 energy
           await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue,base.Owner);

           //Draw 2(3) cards
           await CardPileCmd.Draw(choiceContext,base.DynamicVars.Cards.IntValue,base.Owner);

           //Apply 5 Drowned to yourself
           await PowerCmd.Apply<DrownedPower>(choiceContext,base.Owner.Creature,base.DynamicVars["DrownedPower"].IntValue,base.Owner.Creature,this);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Cards.UpgradeValueBy(1);
        }
    }
}
