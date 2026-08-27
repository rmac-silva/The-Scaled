using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
      
public class Retaliate : TheScaledCard
    {
        public override bool GainsBlock => true;
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(5m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("StrengthLoss",2)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                if (base.CombatState == null)
                {
                    return false;
                }
                return base.CombatState.HittableEnemies.Any((Creature e) => e.Monster?.IntendsToAttack ?? false);
            }
        }
        public Retaliate() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
        {
        }


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            ArgumentNullException.ThrowIfNull(cardPlay.Target.Monster, "cardPlay.Target.Monster");
            await CreatureCmd.GainBlock(base.Owner.Creature, DynamicVars.Block, cardPlay);

            if(cardPlay.Target.Monster.IntendsToAttack)
            {
                ModLog.Info(this, $"Applying Retaliate: {DynamicVars["StrengthLoss"].BaseValue}");
                await PowerCmd.Apply<RetaliatePower>(choiceContext, cardPlay.Target, DynamicVars["StrengthLoss"].BaseValue, base.Owner.Creature, cardPlay.Card);
            }
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars["StrengthLoss"].UpgradeValueBy(1);
        }
    }
}