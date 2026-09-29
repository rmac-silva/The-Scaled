using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers.Cards;


  
public class AdvantagePower : TheScaledPower
{
    private bool _hasTriggeredThisTurn = false;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SetupPower>()];
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
	{
		if (card.Owner.Creature != base.Owner)
		{
			return playCount;
		}
		if (card is not SetupCard)
		{
			return playCount;
		}
        if (_hasTriggeredThisTurn)
        {
            return playCount;
        }

        _hasTriggeredThisTurn = true;
		return playCount + 1;
	}

    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if(side == CombatSide.Player)
        {
            _hasTriggeredThisTurn = false;
        }
        return Task.CompletedTask;
    }

}
