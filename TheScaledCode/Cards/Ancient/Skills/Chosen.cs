using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Ancients;


  
public class Chosen : AncientCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal,CardKeyword.Exhaust];

	public Chosen()
		: base(1, CardType.Skill, CardRarity.Ancient, TargetType.None)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int spaceInHand = CardPile.MaxCardsInHand - PileType.Hand.GetPile(base.Owner).Cards.Count;

		List<CardModel> drawPileRares = PileType.Draw.GetPile(base.Owner).Cards.Where((CardModel c) => c.Rarity == CardRarity.Rare).ToList();
		List<CardModel> discardPileRares = PileType.Discard.GetPile(base.Owner).Cards.Where((CardModel c) => c.Rarity == CardRarity.Rare).ToList();
		List<CardModel> exhaustPileRares = PileType.Exhaust.GetPile(base.Owner).Cards.Where((CardModel c) => c.Rarity == CardRarity.Rare).ToList();

		List<CardModel> allCards = drawPileRares.Union(discardPileRares.Union(exhaustPileRares)).ToList();

		IEnumerable<CardModel> handCards = allCards.TakeRandom(spaceInHand, base.Owner.RunState.Rng.CombatCardSelection);

		foreach (var c in handCards)
		{
			await CardPileCmd.Add(c,PileType.Hand);
		}

		var cardsToDiscard = allCards.Except(handCards).ToList();

		foreach (var c in cardsToDiscard)
		{
			await CardPileCmd.Add(c,PileType.Discard);
		}


	}

	protected override void OnUpgrade()
	{
		base.RemoveKeyword(CardKeyword.Ethereal);
	}

	

	
}