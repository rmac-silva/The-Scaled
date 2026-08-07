using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheScaled.TheScaledCode.Powers;

public class SmotheringPresencePower : TheScaledPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("VulnerableApplied", 1m), new DynamicVar("DrownConsumed", 0m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<DrownedPower>()];

    /// <summary>
    /// Triggers after the player does damage. We will check if the enemy attacked has the Drowned Power, if so, we apply
    /// {Amount} Vulnerable, and remove {Amount} of Drowned
    /// </summary>
    /// <param name="choiceContext"></param>
    /// <param name="dealer"></param>
    /// <param name="result"></param>
    /// <param name="props"></param>
    /// <param name="target"></param>
    /// <param name="cardSource"></param>
    /// <returns></returns>
    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource
    )
    {
        if (!target.HasPower<DrownedPower>())
        {
            return;
        }

        if(props.HasFlag(ValueProp.Unpowered))
        {
            return;
        }

        //Target has Drowned, apply 1 vulnerable
        await PowerCmd.Apply<VulnerablePower>(
            choiceContext,
            target,
            (decimal)(Amount * DynamicVars["VulnerableApplied"].IntValue),
            dealer,
            null
        );

        //Now remove Drowned based on calculated variable
        await PowerCmd.Apply<DrownedPower>(
            choiceContext,
            target,
            -base.DynamicVars["DrownConsumed"].IntValue,
            dealer,
            null
        );

        Flash();
    }

    public void IncrementDrownReduction(int amount)
    {
        base.DynamicVars["DrownConsumed"].BaseValue += amount;
    }
}
