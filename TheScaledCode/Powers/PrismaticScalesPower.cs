using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheScaled.TheScaledCode.Powers;

  
public class PrismaticScalesPower : TheScaledPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<DrownedPower>()];

    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if(amount <= 0)
        {
            return;
        }
        
        //! Only from powered sources like cards. Check if needed
        // if(props.HasFlag(ValueProp.Unpowered))
        // {
        //     return;
        // }

        //Fetch combat participants
        ArgumentNullException.ThrowIfNull(creature.CombatState, "creature.CombatState");
        IReadOnlyList<Creature> p = creature.CombatState.Enemies;

        //Apply drowned to all participants
        await PowerCmd.Apply<DrownedPower>(new ThrowingPlayerChoiceContext(),p,base.Amount,base.Owner,null);
    }
}