using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{

  
  
  
public class GaspingRecess : TheScaledCard
    {
        public GaspingRecess() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override bool ShouldGlowGoldInternal
        {
        get
        {
            if (base.CombatState == null)
            {
                return false;
            }
            return base.CombatState.HittableEnemies.Any((Creature e) => e.HasPower<DrownedPower>());
        }
    }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new PowerVar<DrownedPower>(5),
                new PowerVar<FrailPower>(1)
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<DrownedPower>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];



        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");
            
            var drownedPower = cardPlay.Target.GetPower<DrownedPower>();

            if(drownedPower is null)
            {
                return;
            }

            int reduction = 0;

            while(reduction < base.DynamicVars["DrownedPower"].IntValue && drownedPower.Amount > 0)
            {
                Creature? enemy = base.Owner.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies);

                if(enemy is null)
                {
                    return;
                }

                await PowerCmd.Apply<FrailPower>(choiceContext,enemy,base.DynamicVars["FrailPower"].IntValue,base.Owner.Creature,this);
                reduction++;            
            }
            
            

            
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars["FrailPower"].UpgradeValueBy(1);
        }
    }
}
