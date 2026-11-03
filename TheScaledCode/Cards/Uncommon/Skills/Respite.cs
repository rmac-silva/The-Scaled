using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
      
public class Respite : SetupCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new EnergyVar(2)
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.ForEnergy(Owner)];

        protected override SetupCardType CardSetupType => SetupCardType.Buff;

        public Respite()
            : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await AddSetup(cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Energy.UpgradeValueBy(1);
        }

        public override async Task AmbushEffect(AmbushMethodInfo info)
        {
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, info.applier!.Player!);
        }
    }
}
