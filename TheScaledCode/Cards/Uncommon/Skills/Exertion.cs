using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
    
public class Exertion : SetupCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new CardsVar(3),
                new EnergyVar(1),
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<Ambush>()];

        protected override SetupCardType CardSetupType => SetupCardType.Buff;

        public Exertion()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CardPileCmd.Draw(choiceContext,base.DynamicVars.Cards.IntValue,base.Owner);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Cards.UpgradeValueBy(1);
        }

        protected override async Task AmbushEffect(AmbushMethodInfo info, Dictionary<string,decimal> _)
        {
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue,base.Owner);
        }
    }
}
