using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheScaled.TheScaledCode.Powers;


public class HuntersMarkPower : TheScaledPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("Applier")];
    private bool _triggered = false;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
	{
        if(applier is null || applier.Player is null) {return Task.CompletedTask;}
		((StringVar)base.DynamicVars["Applier"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, Applier!.Player!.NetId);
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
        _triggered = true;
		return 2m;
	}

    public override async Task AfterModifyingDamageAmount(CardModel? cardSource)
    {
        if(_triggered) { 
            await PowerCmd.Decrement(this);
            _triggered = false;
        }
    }
}