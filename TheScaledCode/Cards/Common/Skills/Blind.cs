using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards
{
      
public class Blind : TheScaledCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new PowerVar<VulnerablePower>(1),
                new CardsVar(1)
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<VulnerablePower>(),HoverTipFactory.FromAffliction<Muddied>().First()];

        public Blind()
            : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target,base.DynamicVars["VulnerablePower"].IntValue,Owner.Creature,this);

            if(IsUpgraded)
            {
                var cards = await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, base.DynamicVars.Cards.IntValue, base.DynamicVars.Cards.IntValue), Muddied.CanAfflictMuddied,this);
                await Mud.MuddyCardsNoVisuals(cards.ToList());
            } else
            {
                IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(base.Owner).Cards.Where((CardModel c) => c.Affliction is null && Muddied.CanAfflictMuddied(c)).TakeRandom(base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardSelection);
                await Mud.MuddyCardsNoVisuals(enumerable.ToList());
            }
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars["VulnerablePower"].UpgradeValueBy(2);
        }
    }
}
