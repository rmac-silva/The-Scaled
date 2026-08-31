using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class ChargeThrough : SetupCard
{
    public ChargeThrough() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    protected override SetupCardType CardSetupType => SetupCardType.Offensive;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("AmbushEffect", 8)];
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
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");

        var enemiesToHit = base.CombatState.HittableEnemies.Where((Creature e) => e.Monster?.IntendsToAttack ?? false);

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingFiltered(enemiesToHit)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await base.ApplyAmbushToAllEnemies(choiceContext, cardPlay);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2);
    }
    protected override Task AmbushEffect(AmbushMethodInfo info)
    {
        throw new NotImplementedException();
    }
}