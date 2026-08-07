using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Powers;

  
public class InceptionPower : TheScaledPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
	{
		modifiedCost = originalCost;
		if (card.Owner.Creature != base.Owner)
		{
			return false;
		}
		if (card.Type != CardType.Skill)
		{
			return false;
		}
		bool flag;
		switch (card.Pile?.Type)
		{
		case PileType.Hand:
		case PileType.Play:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (!flag)
		{
			return false;
		}
		ModLog.Info(this,$"Modifying cost of {card} from {originalCost} to {Math.Max(0, originalCost - base.Amount)} based on {base.Amount} Amount.");
		modifiedCost = Math.Max(0, originalCost - base.Amount);
		return true;
	}

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner.Creature == base.Owner && cardPlay.Card.Type == CardType.Skill)
		{
			bool flag;
			switch (cardPlay.Card.Pile?.Type)
			{
			case PileType.Hand:
			case PileType.Play:
				flag = true;
				break;
			default:
				flag = false;
				break;
			}
			if (flag)
			{
                //Remove the power
				await PowerCmd.Apply<InceptionPower>(new ThrowingPlayerChoiceContext(),base.Owner, -base.Amount, base.Owner,null);
			}
		}
	}
}