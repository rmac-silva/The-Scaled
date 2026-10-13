using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Cards;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Ancients;
  
public class ClimaxJumping : AncientCard
{
	public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];

	protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(15m, ValueProp.Move), new DynamicVar("PowerAmount",1), new DynamicVar("DamageMult",2), new DynamicVar("PlayMult",1)];

	public ClimaxJumping()
		: base(3, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this,cardPlay).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);

		(await PowerCmd.Apply<ClimaxJumpingPower>(choiceContext, cardPlay.Target, base.DynamicVars["PowerAmount"].IntValue,base.Owner.Creature,this))?.AddDamageMult(base.DynamicVars["DamageMult"].IntValue).AddPlayMult(base.DynamicVars["PlayMult"].IntValue);
		
	}

    

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(8m);
	}

	
}