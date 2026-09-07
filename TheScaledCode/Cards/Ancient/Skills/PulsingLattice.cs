using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Ancients;

//  (2E): Retain. Choose a card in your Draw Pile without Replay. It gains Replay 4(5) and costs 1 additional Energy.


  
  
public class PulsingLattice : AncientCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain),HoverTipFactory.ForEnergy(base.Owner),HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];

	protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Replay",4), new EnergyVar(1), new CardsVar(1)];

	public PulsingLattice()
		: base(2, CardType.Skill, CardRarity.Ancient, TargetType.None)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		List<CardModel> selection = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(base.Owner), base.Owner, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, base.DynamicVars.Cards.IntValue), CanBeEnchanted())).ToList();
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

		foreach (CardModel item in selection)
		{
			item.BaseReplayCount += base.DynamicVars["Replay"].IntValue;
			item.EnergyCost.AddThisCombat(1);
		}
	}

   

	protected override void OnUpgrade()
	{
		base.DynamicVars["Replay"].UpgradeValueBy(1m);
	}

	/// <summary>
	/// Can't have the Replay enchantment, and must be a playable card.
	/// </summary>
	/// <returns></returns>
	private static Func<CardModel, bool> CanBeEnchanted()
	{
		return card => card != null && card.GetEnchantedReplayCount() < 1 && !card.Keywords.Contains(CardKeyword.Unplayable);
	}

	
}