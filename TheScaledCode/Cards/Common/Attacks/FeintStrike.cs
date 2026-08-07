using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Helpers;

namespace TheScaled.TheScaledCode.Cards
{
    public class FeintStrike : TheScaledCard
    {
        public FeintStrike()
            : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                if (base.CombatState == null || base.CombatState.HittableEnemies == null)
                {
                    return false;
                }

                return base.CombatState.HittableEnemies.Any(
                    (Creature e) =>
                    {
                        if (e.Monster != null)
                        {
                            return IntentHelper.IntendsToBlock(e.Monster.NextMove);
                        }
                        else
                        {
                            return false;
                        }
                    }
                );
            }
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new DamageVar(4m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
                new PowerVar<VulnerablePower>(1),
            ];
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<VulnerablePower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            ArgumentNullException.ThrowIfNull(cardPlay.Target.Monster, "cardPlay.Target.Monster");

            await DamageCmd
                .Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            if (IntentHelper.IntendsToBlock(cardPlay.Target.Monster.NextMove))
            {
                await PowerCmd.Apply<VulnerablePower>(
                    choiceContext,
                    cardPlay.Target,
                    base.DynamicVars.Vulnerable.BaseValue,
                    base.Owner.Creature,
                    this
                );
            }
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
