using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;

/// <summary>
///
/// Note: This needs to have a minimum value of 1, otherwise you will infinitely add strength
/// to your character with any attack as it is always over the threshold.
/// </summary>
public class ToughenUpPower : TheScaledPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar("DexterityGain", 1m),
            new DynamicVar("DexterityGainUpg", 2m),
            new DynamicVar("ThornsGain", 2m),
            new DynamicVar("ThornsGainUpg", 3m),
        ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<DexterityPower>(),
            HoverTipFactory.FromPower<ThornsPower>(),
            HoverTipFactory.FromCard<ToughenUp>(),
        ];

    protected virtual bool IsPositive => true;

    private List<CardModel> cardSources = new List<CardModel>(5);

    private int Sign
    {
        get
        {
            if (!IsPositive)
            {
                return -1;
            }
            return 1;
        }
    }

    public override async Task BeforeApplied(
        Creature target,
        decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        if (cardSource is null)
        {
            ModLog.Warning(
                this,
                "Card source was null! Cannot add ToughenUpPower without a card source."
            );
            return;
        }

        await PowerCmd.Apply<DexterityPower>(
            new ThrowingPlayerChoiceContext(),
            target,
            (decimal)Sign
                * (
                    cardSource.IsUpgraded
                        ? base.DynamicVars["DexterityGainUpg"].BaseValue
                        : base.DynamicVars["DexterityGain"].BaseValue
                )
                * amount, //Applies 1(2)
            applier,
            cardSource,
            silent: true
        );

        // ModLog.Info(
        //     this,
        //     $"Applying Thorns: {(cardSource.IsUpgraded ? DynamicVars["ThornsGainUpg"].IntValue : DynamicVars["ThornsGain"].IntValue) * amount}"
        // );

        await PowerCmd.Apply<ThornsPower>(
            new ThrowingPlayerChoiceContext(),
            target,
            (decimal)Sign
                * (
                    cardSource.IsUpgraded
                        ? base.DynamicVars["ThornsGainUpg"].BaseValue
                        : base.DynamicVars["ThornsGain"].BaseValue
                )
                * amount, //Applies 1(3)
            applier,
            cardSource,
            silent: true
        );

        //Add it to the card sources
        cardSources.Add(cardSource);
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        if (!(amount == (decimal)base.Amount) && power == this) //Non-null change
        {
            // ModLog.Info(
            //     this,
            //     $"Applying Dexterity: {(cardSource.IsUpgraded ? DynamicVars["DexterityGainUpg"].IntValue : DynamicVars["DexterityGain"].IntValue) * amount}"
            // );

            if (cardSource is null)
            {
                ModLog.Warning(
                    this,
                    "Card source was null! Cannot add ToughenUpPower without a card source."
                );
                return;
            }

            await PowerCmd.Apply<DexterityPower>(
                new ThrowingPlayerChoiceContext(),
                base.Owner,
                (decimal)Sign
                    * (
                        cardSource.IsUpgraded
                            ? DynamicVars["DexterityGainUpg"].IntValue
                            : DynamicVars["DexterityGain"].IntValue * amount
                    ), //Applies 1(2)
                applier,
                cardSource,
                silent: true
            );
            // ModLog.Info(
            //     this,
            //     $"Applying Thorns: {(cardSource.IsUpgraded ? DynamicVars["ThornsGainUpg"].IntValue : DynamicVars["ThornsGain"].IntValue) * amount}"
            // );

            await PowerCmd.Apply<ThornsPower>(
                new ThrowingPlayerChoiceContext(),
                base.Owner,
                (decimal)Sign
                    * (
                        cardSource.IsUpgraded
                            ? DynamicVars["ThornsGainUpg"].IntValue
                            : DynamicVars["ThornsGain"].IntValue * amount
                    ), //Applies 1(3)
                applier,
                cardSource,
                silent: true
            );

            //Add it to the card sources
            cardSources.Add(cardSource);
        }
    }

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState
    )
    {
        if (side == CombatSide.Player)
        {
            //Remove applied powers
            foreach (var src in cardSources)
            {
                if (src is null)
                {
                    ModLog.Warning(this, "Card source was null!");
                    //Ignore this src
                    continue;
                }

                if (base.Owner.GetPower<DexterityPower>() is null)
                {
                    ModLog.Warning(this, "Dexterity power was null!");
                    continue;
                }

                if (base.Owner.GetPower<ThornsPower>() is null)
                {
                    ModLog.Warning(this, "Thorns power was null!");
                    continue;
                }

                // ModLog.Info(this, $"Card source is upgraded: {src.IsUpgraded}, which will result in a dexterity loss of {(src.IsUpgraded ? DynamicVars["DexterityGainUpg"].IntValue : DynamicVars["DexterityGain"].IntValue)} and a thorns loss of {(src.IsUpgraded ? DynamicVars["ThornsGainUpg"].IntValue : DynamicVars["ThornsGain"].IntValue)}");

                await PowerCmd.Apply<DexterityPower>(
                    new ThrowingPlayerChoiceContext(),
                    base.Owner,
                    -(decimal)Sign
                        * (
                            src.IsUpgraded
                                ? DynamicVars["DexterityGainUpg"].IntValue
                                : DynamicVars["DexterityGain"].IntValue
                        ),
                    null,
                    src
                );
                await PowerCmd.Apply<ThornsPower>(
                    new ThrowingPlayerChoiceContext(),
                    base.Owner,
                    -(decimal)Sign
                        * (
                            src.IsUpgraded
                                ? DynamicVars["ThornsGainUpg"].IntValue
                                : DynamicVars["ThornsGain"].IntValue //We need to multiply by 3 here because the thorns gain is 3xAmount, and the modify amount method will not subtract the base amount.
                        ),
                    null,
                    src
                );
            }

            base.RemoveInternal();
            cardSources.Clear();
        }
    }
}
