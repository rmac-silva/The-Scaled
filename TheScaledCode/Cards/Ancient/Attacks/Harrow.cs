

using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Ancients;

// (2E) Deal 54(63) damage. If Fatal gain 66(99) Gold.


public class Harrow : AncientCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(25m, ValueProp.Move),
		new CalculationBaseVar(0),
		new ExtraDamageVar(1m),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? target) => target?.Powers.Count(ShouldCountPower) ?? 0)
	];

	public Harrow()
		: base(2, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

		var hitCount = cardPlay.Target.Powers.Count(ShouldCountPower);

		await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
			.WithHitCount(hitCount)
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.ExtraDamage.UpgradeValueBy(3m);
		base.DynamicVars.CalculationBase.UpgradeValueBy(3m);
	}

	private static bool ShouldCountPower(PowerModel power)
	{
		if (power.TypeForCurrentAmount == PowerType.Debuff)
		{
			return !(power is ITemporaryPower);
		}
		return false;
	}

	
}