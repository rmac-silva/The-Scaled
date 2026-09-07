using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Afflictions;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{

  
  
  
  
  
  
  
public class Riptide : TheScaledCard
    {
        public Riptide() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
        {
        }

        protected override bool HasEnergyCostX => true;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<DrownedPower>()];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrownedPower>(3)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(base.RunState);
            ArgumentNullException.ThrowIfNull(base.CombatState);

            int num = ResolveEnergyXValue();

            for (int i = 0; i < num; i++)
            {
                var enemy = base.RunState.Rng.CombatTargets.NextItem(base.CombatState.Enemies);

                if(enemy is null) {continue;}
                await PowerCmd.Apply<DrownedPower>(choiceContext,enemy,base.DynamicVars["DrownedPower"].IntValue,base.Owner.Creature,this);
            }
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
