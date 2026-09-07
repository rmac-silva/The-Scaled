using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Ancients;

//   (3E): (Retain.) Play EVERY Attack from your Discard Pile.
public class Overwhelm : AncientCard
{

	public Overwhelm()
		: base(3, CardType.Skill, CardRarity.Ancient, TargetType.None)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(base.CombatState,"base.CombatState");

		CardPile pile = PileType.Discard.GetPile(base.Owner);
		IEnumerable<CardModel> enumerable = pile.Cards.Where((CardModel c) => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList().StableShuffle(base.Owner.RunState.Rng.Shuffle);

		foreach (CardModel item in enumerable)
		{
		if (!CombatManager.Instance.IsOverOrEnding)
		{
			if (item.TargetType == TargetType.AnyEnemy)
			{
				Creature? target = base.Owner.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies);
				await CardCmd.AutoPlay(choiceContext, item, target);
			}
			else
			{
				await CardCmd.AutoPlay(choiceContext, item, null);
			}
			continue;
		}
		break;
		}
	}

   

	protected override void OnUpgrade()
	{
		base.AddKeyword(CardKeyword.Retain);
	}

	

	
}