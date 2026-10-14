using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Powers;


public class TidalShieldPower : TheScaledPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if(dealer == Owner && result.UnblockedDamage > 0 && HasDrown(target))
        {
            CreatureCmd.GainBlock(Owner,Amount,ValueProp.Unpowered,null);
        }
        return Task.CompletedTask;
    }

    private bool HasDrown(Creature creature)
    {
        return creature.HasPower<DrownedPower>();
    }
}