using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
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

  
public class PersistDrownedPower : TheScaledPower
{

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStartLate(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(base.Applier))
		{
			await PowerCmd.Decrement(this);
		}
    }

    

}
