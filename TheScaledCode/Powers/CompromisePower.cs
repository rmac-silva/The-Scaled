using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Powers;

/// <summary>
/// This power represents the power of the Compromise card, increasing the exertion by 1
/// across {Amount} turns
/// </summary>
  
public class CompromisePower : TheScaledPower
{
    private bool _shouldIgnoreNextInstance;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<ExertionPower>()];

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

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public void IgnoreNextInstance()
    {
        _shouldIgnoreNextInstance = false;
    }

    public override async Task BeforeApplied(
        Creature target,
        decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            ModLog.Info(
                this,
                $"Compromise Power has been applied by {amount}."
                );
            await PowerCmd.Apply<ExertionPower>(
                new ThrowingPlayerChoiceContext(),
                target,
                (decimal)Sign * 1, //Always applies 1 Exertion increase
                applier,
                cardSource,
                silent: true
            );
        }
    }


    // public override async Task AfterPowerAmountChanged(
    //     PlayerChoiceContext choiceContext,
    //     PowerModel power,
    //     decimal amount,
    //     Creature? applier,
    //     CardModel? cardSource
    // )
    // {
    //     if (!(amount == (decimal)base.Amount) && power == this) //Non-null change
    //     {
    //         if (_shouldIgnoreNextInstance)
    //         {
    //             _shouldIgnoreNextInstance = false;
    //         }
    //         else
    //         {
    //             ModLog.Info(
    //             this,
    //             $"Temporary Exertion Power has changed by {amount}."
    //             );
    //             await PowerCmd.Apply<ExertionPower>(
    //                 choiceContext,
    //                 base.Owner,
    //                 (decimal)Sign * 1,
    //                 applier,
    //                 cardSource,
    //                 silent: true
    //             );
    //         }
    //     }
    // }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side == CombatSide.Enemy)
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
                -(decimal)Sign * 1, //Remove 1 Exertion
                null,
                null,
                silent: true
            );
    }

    
}
