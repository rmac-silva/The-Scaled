using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards
{
      
  
public class Trip : TheScaledCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new PowerVar<VulnerablePower>(1),
                new PowerVar<DrownedPower>(4)
            ];
        protected override bool ShouldGlowGoldInternal => base.CombatState?.HittableEnemies.Any((Creature e) => !e.HasPower<DrownedPower>()) ?? false;
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<DrownedPower>()];

        public Trip()
            : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target,base.DynamicVars["VulnerablePower"].IntValue,Owner.Creature,this);
        
            if(cardPlay.Target.GetPower<DrownedPower>() is null)
            {
                await PowerCmd.Apply<DrownedPower>(choiceContext, cardPlay.Target,base.DynamicVars["DrownedPower"].IntValue,Owner.Creature,this);
            }
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Vulnerable.UpgradeValueBy(1);
            base.DynamicVars["DrownedPower"].UpgradeValueBy(2);
        }
    }
}
