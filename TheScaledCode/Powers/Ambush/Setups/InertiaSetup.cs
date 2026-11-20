using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheScaled.TheScaledCode.Powers;




  
  

public class InertiaSetup : BaseSetupPower
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DamageIncrease", 0), new DamageVar(0,ValueProp.Unpowered)];
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        await CreatureCmd.Damage(ctx, Owner, DynamicVars.Damage, Applier!);
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if(side == CombatSide.Player)
        {
            DynamicVars.Damage.BaseValue += DynamicVars["DamageIncrease"].IntValue;
        }
        return Task.CompletedTask;
    }

    public void SetDamageIncrease(int damageIncrease)
    {
        DynamicVars["DamageIncrease"].BaseValue = damageIncrease;
        DynamicVars.Damage.BaseValue = damageIncrease;
    }
    protected override void AfterCloned()
    {
        DynamicVars["DamageIncrease"].BaseValue = 0;
        DynamicVars.Damage.BaseValue = 0;
    }

}