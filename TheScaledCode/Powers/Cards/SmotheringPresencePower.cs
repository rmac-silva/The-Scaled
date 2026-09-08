using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Ancients;
using TheScaled.TheScaledCode.Cards;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Powers.Cards;


public class SmotheringPresencePower : TheScaledPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<DrownedPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DrownConsumed", 0)];


    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        //Took 0 damage, don't trigger the power
        if (result.UnblockedDamage <= 0 || result.Props.HasFlag(ValueProp.Unpowered))
        {
            return;
        }
        else
        {
            if (target.HasPower<DrownedPower>())
            {
                await PowerCmd.Apply<VulnerablePower>(choiceContext, target, base.Amount, base.Owner, null);

                //Manually decrement Drowned based on accumulated value
                await PowerCmd.Apply<DrownedPower>(choiceContext, target, -base.DynamicVars["DrownConsumed"].IntValue, base.Owner, null);
            }
        }
    }

    /// <summary>
    /// Method to stack SmotheringPresencePower. Increasing the amount of Drown consumed when stacking powers.
    /// </summary>
    /// <param name="drownConsumed"></param>
    public void AddDrownConsumption(int drownConsumed)
    {
        base.DynamicVars["DrownConsumed"].BaseValue += drownConsumed;
    }

}
