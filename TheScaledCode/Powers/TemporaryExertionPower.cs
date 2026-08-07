using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;

/// <summary>
/// This power represents a single-turn exertion change of {Amount}.
/// </summary>
public class TemporaryExertionDownPower : TheScaledPower, ITemporaryPower
{
    private bool _shouldIgnoreNextInstance;
    public AbstractModel OriginModel => ModelDb.Card<Respite>();

    public PowerModel InternallyAppliedPower => ModelDb.Power<ExertionPower>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<ExertionPower>()];

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
                $"Temporary Exertion Power has been applied by {amount}."
                );
            await PowerCmd.Apply<ExertionPower>(
                new ThrowingPlayerChoiceContext(),
                target,
                (decimal)Sign * amount,
                applier,
                cardSource,
                silent: true
            );
        }
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        if (!(amount == (decimal)base.Amount) && power == this) //Non-null change
        {
            if (_shouldIgnoreNextInstance)
            {
                _shouldIgnoreNextInstance = false;
            }
            else
            {
                ModLog.Info(
                this,
                $"Temporary Exertion Power has changed by {amount}."
                );
                await PowerCmd.Apply<ExertionPower>(
                    choiceContext,
                    base.Owner,
                    (decimal)Sign * amount,
                    applier,
                    cardSource,
                    silent: true
                );
            }
        }
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants
    )
    {
        if (participants.Contains(base.Owner))
        {
            Flash();
            await PowerCmd.Remove(this);
            await PowerCmd.Apply<ExertionPower>(
                choiceContext,
                base.Owner,
                -Sign * base.Amount,
                base.Owner,
                null
            );
        }
    }
}
