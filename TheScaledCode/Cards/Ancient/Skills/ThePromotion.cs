using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Ancients;


  
  
public class ThePromotion : AncientCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(75,ValueProp.Move), new DynamicVar("Blur",1)];

	public ThePromotion()
		: base(0, CardType.Skill, CardRarity.Ancient, TargetType.None)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
		await PowerCmd.Apply<BlurPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Blur"].BaseValue, base.Owner.Creature, this);
		await PowerCmd.Apply<TheGambitPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Block.UpgradeValueBy(25);
	}

	

	
}