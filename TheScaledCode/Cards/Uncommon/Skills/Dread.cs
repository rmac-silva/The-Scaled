using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheScaled.TheScaledCode.Cards
{
    public class Dread : TheScaledCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new PowerVar<FrailPower>(99),
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<FrailPower>(),HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        public Dread()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await PowerCmd.Apply<FrailPower>(choiceContext, cardPlay.Target, base.DynamicVars["FrailPower"].IntValue, base.Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
