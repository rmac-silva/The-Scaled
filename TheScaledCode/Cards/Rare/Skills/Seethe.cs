using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;
using TheScaled.TheScaledCode.Powers.Cards;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards
{

public class Seethe : TheScaledCard
    {
        public Seethe() : base(3, CardType.Skill, CardRarity.Rare, TargetType.None)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new CalculationBaseVar(0),
                new CalculationExtraVar(1),
                new CalculatedVar("ReplayAmount").WithMultiplier( (CardModel card, Creature? _) => CardPile.Get(PileType.Discard,card.Owner)?.Cards.Count((CardModel c) => c.Affliction != null && c.Affliction is Muddied) ?? 0)
            ];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromAffliction<Muddied>(1).First(), HoverTipFactory.Static(StaticHoverTip.ReplayStatic), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
           var numReplay = CardPile.Get(PileType.Discard,base.Owner)?.Cards.Count((CardModel c) => c.Affliction != null && c.Affliction is Muddied) ?? 0;
           CardModel? cardModel = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(base.Owner), base.Owner, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1))).FirstOrDefault();

            if(cardModel != null)
            {
                cardModel.BaseReplayCount += numReplay;
                CardCmd.Preview(cardModel);
            }
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }

        
    }
}
