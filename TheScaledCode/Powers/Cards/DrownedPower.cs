using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheScaled.TheScaledCode.Powers;

/// <summary>
///
/// Note: This needs to have a minimum value of 1, otherwise you will infinitely add strength
/// to your character with any attack as it is always over the threshold.
/// </summary>

public class DrownedPower : TheScaledPower
{

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        //Took 0 damage, don't trigger the power
        if(result.UnblockedDamage <= 0 || result.Props.HasFlag(ValueProp.Unpowered) )
        {
            return;
        } else
        {
            //Deal damage to the enemy equal to the power amount
            if(target == base.Owner)
            {
                //The target is the owner of the power.
                //Damage him according to the amount of the power.
                await CreatureCmd.Damage(choiceContext,base.Owner,base.Amount,ValueProp.Unblockable | ValueProp.Unpowered,base.Owner);

                //Tick down the power by 1
                await PowerCmd.Decrement(this);
            }
        }
    }

}
