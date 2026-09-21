using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;



public class BellyFlop : TheScaledCard
{
    public BellyFlop() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(28, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        ArgumentNullException.ThrowIfNull(this.CombatState, "this.CombatState");

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(this.CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        var allEnemies = this.CombatState.Enemies;
        foreach (var e in allEnemies)
        {
            var ambushPower =e.GetPower<Ambush>();
            if(ambushPower != null)
            {
                ambushPower.SetAmount(0);
            }
        }

    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(8);
    }


}