using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheScaled.TheScaledCode.Cards;

  
public class TailSweep : TheScaledCard
{
    public TailSweep() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");


        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
        .FromCard(this)
        .TargetingAllOpponents(base.CombatState)
        .WithHitCount(2)
        .WithHitFx("vfx/vfx_attack_slash")
        .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3);
    }

}