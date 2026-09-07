using MegaCrit.Sts2.Core.CardSelection;
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

  
  
public class Bathe : TheScaledCard
    {
        public Bathe() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new CardsVar(1),
                new PowerVar<DrownedPower>(3),
            ];
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<DrownedPower>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];



        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");
            
            CardModel? exhaustedCard = (await CardSelectCmd.FromCombatPile(prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, 1), context: choiceContext, pile: PileType.Discard.GetPile(base.Owner), player: base.Owner)).FirstOrDefault();
            if (exhaustedCard != null)
            {
                await CardCmd.Exhaust(choiceContext,exhaustedCard);
            }

            await PowerCmd.Apply<DrownedPower>(choiceContext,base.CombatState.Enemies,base.DynamicVars["DrownedPower"].IntValue,base.Owner.Creature,this);
            
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars["DrownedPower"].UpgradeValueBy(1);
        }
    }
}
