using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Powers;

/// <summary>
/// This power represents the power of the Compromise card, increasing the exertion by 1
/// across {Amount} turns
/// </summary>
  
  
public class InertiaPower : TheScaledPower
{
    private bool _shouldIgnoreNextInstance;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<ExertionPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("ReductionAmount",0m)];

    protected virtual bool IsPositive => false;

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

    public void IgnoreNextInstance()
    {
        _shouldIgnoreNextInstance = false;
    }
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {

        //Amount only represents how many turns this power will last. 
        //So if it was a negative gain, we are simply ticking down turns.
        if(power != this || amount <= 0)
        {
            return;
        }

        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<ExertionPower>(
                new ThrowingPlayerChoiceContext(),
                base.Owner,
                (decimal)Sign * 1, //Always lowers Exertion by 1, per instance
                applier,
                cardSource,
                silent: true
            );
        }
    }
    


    public void IncreaseReductionAmount()
    {
        base.DynamicVars["ReductionAmount"].BaseValue++;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side == CombatSide.Player)
		{
			await PowerCmd.TickDownDuration(this);
		}
	}

    public override async Task AfterRemoved(Creature oldOwner)
    {
        await base.AfterRemoved(oldOwner);

            await PowerCmd.Apply<ExertionPower>(
                new ThrowingPlayerChoiceContext(),
                oldOwner,
                -(decimal)Sign * base.DynamicVars["ReductionAmount"].IntValue, //Remove 1 Exertion
                null,
                null,
                silent: true
            );
    }

    
}
