using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheScaled.TheScaledCode.Cards;

  
public class ChargeThrough : TheScaledCard
{
    public ChargeThrough() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.None)
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
            return base.CombatState.HittableEnemies.Any(
                (Creature e) =>
                {
                    if (e.Monster is not null)
                    {
                        return e.Monster.IntendsToAttack;
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
            new DamageVar(14,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");

        var targets = base.CombatState.HittableEnemies.Where((Creature e) =>{if (e.Monster is not null){return e.Monster.IntendsToAttack;}else{return false;}});
    
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
        .FromCard(this)
        .TargetingFiltered(targets)
        .WithHitFx("vfx/vfx_attack_slash")
        .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(4);
    }
}