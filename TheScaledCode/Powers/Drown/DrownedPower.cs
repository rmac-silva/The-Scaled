using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheScaled.TheScaledCode.Powers.ReusablePowers;

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
        if(result.Props.HasFlag(ValueProp.Unpowered) || result.TotalDamage <= 0 )
        {
            return;
        } else
        {
            //Deal damage to the enemy equal to the power amount
            if(target == base.Owner)
            {
                await Cmd.Wait(0.3f);
                
                await CreatureCmd.Damage(choiceContext, base.Owner, base.Amount, ValueProp.Unpowered, base.Owner);

                //Tick down the power by 1
                await PowerCmd.Decrement(this);
            }
        }
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if(player != base.Owner.Player)
        {
            return;
        }
        
        if(base.Owner.HasPower<PersistDrownedPower>())
        {
            //Do nothing, the power is persistent
        } else if(CombatState.Allies.Any( (Creature c) => c.HasPower<SmotheringPresencePower>()))
        {
            SetAmount((int)Math.Floor(base.Amount / 2.0));
            InvokeDisplayAmountChanged();
        }
        else {
            await PowerCmd.Remove(this);
        }
        
    }



    

}
