using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards
{
      
  
public class Soak : TheScaledCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new EnergyVar(1)
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromAffliction<Muddied>().First()];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];



        public Soak()
            : base(0, CardType.Skill, CardRarity.Common, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, Owner);
            var cards = CardPile.Get(PileType.Hand, Owner)!.Cards.Where(Muddied.CanAfflictMuddied);

            if(cards == null || cards.Count() == 0)
            {
                return;
            }

            await Mud.MuddyCardsNoVisuals(cards.ToList());
            await CardCmd.Discard(choiceContext, cards);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Energy.UpgradeValueBy(1);
        }
    }
}
