using System.IO.Pipes;
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

public class Impede : TheScaledCard
    {
        public Impede() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
        {}

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new CardsVar(1)
        ];  


        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

            
            //Choose card in draw pile to move to discard pile
            CardModel? cardModel = (await CardSelectCmd.FromCombatPile(prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, 1), context: choiceContext, pile: PileType.Draw.GetPile(base.Owner), player: base.Owner)).FirstOrDefault();
            if (cardModel != null)
            {
                await CardPileCmd.Add(cardModel, PileType.Discard);
            }
        }

        protected override void OnUpgrade()
        {
            base.RemoveKeyword(CardKeyword.Exhaust);
        }
    }
}