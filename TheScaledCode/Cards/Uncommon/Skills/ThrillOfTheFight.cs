using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{

  
  
  
  
  
  
public class ThrillOfTheFight : TheScaledCard
    {
        public ThrillOfTheFight() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<Ambush>(),HoverTipFactory.ForEnergy(this)];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            var ambPwr = SetupCard.GetAmbushPowerForApplier(base.Owner.Creature,cardPlay.Target);

            if(ambPwr is null)
            {
                return;
            }
            int currentAmount = ambPwr.Amount;
            for (int i = 0; i < currentAmount; i++)
            {
                ambPwr.SetAmount(ambPwr.Amount - 1);
                await PlayerCmd.GainEnergy(1,base.Owner);
            }
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
