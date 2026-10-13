using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;

public class NaturalHabitatPower : TheScaledPower
{
    private class Data
	{
		public int mudPlayedThisTurn;
	}

    public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    protected override object InitInternalData()
	{
		return new Data();
	}

    public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
	{
		modifiedCost = originalCost;
		if (ShouldSkip(card))
		{
			return false;
		}
		modifiedCost = originalCost - 1;
		return true;
	}

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner.Creature == base.Owner && cardPlay != null && !cardPlay.IsAutoPlay && cardPlay.IsLastInSeries && Mud.HasMuddied(cardPlay.Card))
		{
			GetInternalData<Data>().mudPlayedThisTurn++;
		}
		
		return Task.CompletedTask;
	}

    private bool ShouldSkip(CardModel card)
	{
		if (card.Owner.Creature != base.Owner)
        {
            return true;
        }

        var pileType = card.Pile?.Type;
        if (pileType != PileType.Hand && pileType != PileType.Play)
        {
            return true;
        }
        if(!Mud.HasMuddied(card))
        {
            return true;
        }

        // Otherwise, check if the mud limit has been reached
        return GetInternalData<Data>().mudPlayedThisTurn >= base.Amount;
	}
}