using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Cards;
using TheScaled.TheScaledCode.Powers.Cards;

namespace TheScaled.TheScaledCode.Ancients;


  
//ALL players gain 24(32) Block. ALL players gain 12 Block at the start of the next 2 turns.

  
public class Marshal : AncientCard
{
	public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
	protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(24,ValueProp.Move),new DynamicVar("BlockPerTurn",12), new DynamicVar("Duration",2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<MarshalPower>()];
	public Marshal()
		: base(2, CardType.Skill, CardRarity.Ancient, TargetType.AllAllies)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(base.CombatState);
		
		IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
			where c != null && c.IsAlive && c.IsPlayer
			select c;
		foreach (Creature item in enumerable)
		{
			await CreatureCmd.GainBlock(item, base.DynamicVars.Block, cardPlay);
			await PowerCmd.Apply<MarshalPower>(choiceContext,item,base.DynamicVars["Duration"].IntValue,base.Owner.Creature,this);
		}
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Block.UpgradeValueBy(8);
	}

	

	
}