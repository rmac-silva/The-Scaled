using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Relics;

  


public class TheLoo : TheScaledRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<DrownedPower>(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];

   public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount, Creature? target, CardModel? cardSource)
	{
		if (!(power is DrownedPower))
		{
			return 0m;
		}
		if (giver != base.Owner.Creature)
		{
			return 0m;
		}
		return base.DynamicVars["DrownedPower"].IntValue;
	}

    public override Task AfterModifyingPowerAmountGiven(PowerModel power)
	{
		Flash();
		return Task.CompletedTask;
	}

}