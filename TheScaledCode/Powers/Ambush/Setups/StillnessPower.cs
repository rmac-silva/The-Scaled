using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Powers;

  
public class StillnessSetup : BaseSetupPower
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DrownIncrease",3)];
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        await PowerCmd.Apply<DrownedPower>(ctx, base.Owner, DynamicVars["DrownedPower"].IntValue, Applier, null);
    }

    public override Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if(dealer == Owner)
        {
            SetAmount(Amount + DynamicVars["DrownIncrease"].IntValue);
        }
        return Task.CompletedTask;
    }


}