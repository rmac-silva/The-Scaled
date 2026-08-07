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
/// Lowers exertion threshold by {Amount} next turn.
/// </summary>
public class BidePower : TheScaledPower
{
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

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState
    )
    {
        if (participants.Contains(base.Owner))
        {
            Flash();
            await PowerCmd.Remove(this);
            await PowerCmd.Apply<TemporaryExertionDownPower>(
                new ThrowingPlayerChoiceContext(),
                base.Owner,
                Sign * base.Amount,
                base.Owner,
                null
            );
        }
    }
}
