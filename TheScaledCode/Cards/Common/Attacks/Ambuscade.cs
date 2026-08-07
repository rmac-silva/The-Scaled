using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class Ambuscade : TheScaledCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(10m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new DynamicVar("DrownAmount", 4m),
        ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];

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
        
    public Ambuscade()
        : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        if (cardPlay.Target.HasPower<DrownedPower>())
        {
            await PowerCmd.Apply<DrownedPower>(
                choiceContext,
                cardPlay.Target,
                base.DynamicVars["DrownAmount"].BaseValue,
                base.Owner.Creature,
                this
            );
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["DrownAmount"].UpgradeValueBy(2);
    }
}
