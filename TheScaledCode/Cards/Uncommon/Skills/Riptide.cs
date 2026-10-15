using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards
{

  
  
  
  
  
  
  
public class Riptide : TheScaledCard
    {
        public Riptide() : base(2, CardType.Skill, CardRarity.Common, TargetType.None)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<DrownedPower>()];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrownedPower>(10)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(base.CombatState);
            
            Creature? enemy = base.Owner.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies);
            if (enemy == null)
			{
				return;
			}
            await PowerCmd.Apply<DrownedPower>(choiceContext, enemy, base.DynamicVars["DrownedPower"].IntValue, base.Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
            base.DynamicVars["DrownedPower"].UpgradeValueBy(5);
        }
    }
}
