using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Ancients;

namespace TheScaled.TheScaledCode.Powers;
  
public sealed class ClimaxJumpingPower : CustomPowerModel
{
	private const string _applierTag = "Applier";

	public override PowerType Type => PowerType.Debuff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("Applier"), new DynamicVar("PlayMult",0), new DynamicVar("DamageMult",0)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<ClimaxJumping>()];
	public override Task AfterApplied(Creature? applier, CardModel? cardSource)
	{
		ArgumentNullException.ThrowIfNull(base.Applier);
		ArgumentNullException.ThrowIfNull(base.Applier.Player);

		((StringVar)base.DynamicVars["Applier"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, base.Applier.Player.NetId);
		return Task.CompletedTask;
	}

	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
	{
		if (target != base.Owner)
		{
			return 1m;
		}
		if (!props.IsPoweredAttack())
		{
			return 1m;
		}
		if (dealer == base.Applier)
		{
			return 1m;
		}
		return base.DynamicVars["DamageMult"].IntValue;
	}

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
	{
		if (target != base.Owner)
		{
			return playCount;
		}
        if (card.Owner.Creature == base.Applier)
		{
			return playCount;
		}
		if (card.Type != CardType.Attack)
		{
			return playCount;
		}

		return playCount + base.DynamicVars["PlayMult"].IntValue;
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (participants.Contains(base.Owner))
		{
			await PowerCmd.Remove(this);
		}
	}

    public ClimaxJumpingPower AddPlayMult(int mult)
    {
        base.DynamicVars["PlayMult"].BaseValue += mult;
        return this;
    }

    public ClimaxJumpingPower AddDamageMult(int mult)
    {
        base.DynamicVars["DamageMult"].BaseValue += mult;
        return this;
    }
}