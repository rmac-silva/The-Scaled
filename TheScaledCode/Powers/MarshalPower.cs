using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Ancients;

namespace TheScaled.TheScaledCode.Powers;
  
  
public class MarshalPower : TheScaledPower
{


    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<Marshal>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12,MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered)];

    protected virtual bool IsPositive => true;

    private int Sign
    {
        get
        {
            if (!IsPositive)
            {
                return -1;
            }
            return 1;
        }
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;


    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if(side == CombatSide.Player)
        {
            await CreatureCmd.GainBlock(base.Owner, base.DynamicVars.Block.IntValue,MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered, null);
            await PowerCmd.TickDownDuration(this);
        }
    }

    

    

    
}
