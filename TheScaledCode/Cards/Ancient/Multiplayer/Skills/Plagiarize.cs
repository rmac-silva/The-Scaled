using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Ancients;
  
  
public class Plagiarize : AncientCard
{
	public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
	public Plagiarize()
		: base(1, CardType.Skill, CardRarity.Ancient, TargetType.AnyAlly)
	{
	}

	protected override IEnumerable<DynamicVar> CanonicalVars => [
	
		new CalculationBaseVar(0m),
		new CalculationExtraVar(1m),
		new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? target) => target?.Block ?? 0)
	];

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

		await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), base.DynamicVars.CalculatedBlock.Props, cardPlay);

		List<PowerModel> originalBuffs = (from p in cardPlay.Target.Powers
			where p.TypeForCurrentAmount == PowerType.Buff
			select (PowerModel)p.ClonePreservingMutability()).ToList();

		foreach (PowerModel item in originalBuffs)
			{
				PowerModel? powerModel = PowerCmd.FindExistingInstanceForStacking(item, base.Owner.Creature, item.Applier);
				if (powerModel != null)
				{
					// DoHackyThingsForSpecificPowers(powerModel);
					await PowerCmd.ModifyAmount(choiceContext, powerModel, item.Amount, item.Applier, this);
				}
				else
				{
					PowerModel power = (PowerModel)item.ClonePreservingMutability();
					// DoHackyThingsForSpecificPowers(power);
					await PowerCmd.Apply(choiceContext, power, base.Owner.Creature, item.Amount, item.Applier, this);
				}
			}
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}

	

	
}