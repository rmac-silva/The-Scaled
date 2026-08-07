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

namespace TheScaled.TheScaledCode.Powers;

/// <summary>
///
/// Note: This needs to have a minimum value of 1, otherwise you will infinitely add strength
/// to your character with any attack as it is always over the threshold.
/// </summary>
public class ExertionPower : TheScaledPower
{
    private class Data
    {
        public int damageThreshold = 0;
        public int damageDealtThisTurn = 0;
    }

    private bool _triggeredThisTurn;

    private Data _Data => GetInternalData<Data>();

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => GetDisplayValue();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("StrengthGain", 1m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<StrengthPower>()];

    /// <summary>
    /// Triggers after the player does damage. We add the total dealt damage to the damageDealtThisTurn variable.
    /// If we are below or equal to the damage threshold, we add one Strength.
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
        //It wasn't this player that dealt damage
        if (dealer == null || dealer != base.Owner)
        {
            return;
        }

        //If it has already triggered this turn
        if(_triggeredThisTurn)
        {
            return;
        }

        //The source of damage wasn't a normal "Powered" attack, or the player dealt 0 or less damage.
        if (!props.IsPoweredAttack() || result.TotalDamage <= 0)
        {
            return;
        }

        
        _Data.damageDealtThisTurn += result.UnblockedDamage;

        ModLog.Info(
            this,
            $"Player dealt {result.TotalDamage} damage! Exertion is at {_Data.damageDealtThisTurn}/{_Data.damageThreshold}."
        );


        if (_Data.damageDealtThisTurn >= _Data.damageThreshold)
        {
            await ApplyStrengthGain();
            _triggeredThisTurn = true;
        }
        InvokeDisplayAmountChanged();
    }

    protected override object InitInternalData()
    {
        var d = new Data();
        d.damageThreshold = base.Amount;
        d.damageDealtThisTurn = 0;

        return d;
    }

    /// <summary>
    /// Resets the counter when the player turn ends.
    /// </summary>
    /// <param name="side"></param>
    /// <param name="participants"></param>
    /// <param name="combatState"></param>
    /// <returns></returns>
    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState
    )
    {
        if (side == CombatSide.Player)
        {
            ModLog.Info(
                this,
                $"Player has started a new turn, resetting damage to be dealt to {base.Amount}!"
            );

            _triggeredThisTurn = false;
            _Data.damageDealtThisTurn = 0;
            ChangeDamageToBeDealt(base.Amount);

        }
    }

    /// <summary>
    /// Runs after the amount of Exertion is changed.
    /// base.Amount gets updated by the game logic, so all we need to do is register
    /// the change in our seperately tracked variable damageToBeDealt.
    ///
    /// We do this by changing the counter based on a value, in this case, the 'amount' that
    /// was changed.
    ///
    /// For example if I play Respite, 'amount = -3' so the counter will decrease by 3
    /// If it's NaturalInstinct triggering, 'amount = 15' then the counter will increase by 15
    /// </summary>
    /// <param name="choiceContext"></param>
    /// <param name="power"></param>
    /// <param name="amount"></param>
    /// <param name="applier"></param>
    /// <param name="cardSource"></param>
    /// <returns></returns>
    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        //If this was the power that was affected
        if (power == this)
        {
            ModLog.Info(
                this,
                $"The amount of this power has changed by {amount}. The new basevalue is: {base.Amount}."
            );

            ChangeCounterByValue(amount);
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// Changes the visual counter that displays how much damage the player needs to deal to trigger
    /// the strength gain
    /// </summary>
    /// <param name="amount"></param>
    private void ChangeDamageToBeDealt(decimal amount)
    {
        Data data = _Data;

        int max = Math.Max(1, base.Amount);
        data.damageThreshold = Math.Clamp((int)amount, 1, max);

        InvokeDisplayAmountChanged();
    }

    /// <summary>
    /// Changes the damageToBeDealt based on a given value.
    /// </summary>
    /// <param name="value"></param>
    private void ChangeCounterByValue(decimal value)
    {
        ChangeDamageToBeDealt(_Data.damageThreshold += (int)value);
    }

    /// <summary>
    /// Gives the player the Strength Power
    /// </summary>
    /// <returns></returns>
    private async Task ApplyStrengthGain()
    {
        Flash();

        //Apply strength power
        await PowerCmd.Apply<StrengthPower>(
            new ThrowingPlayerChoiceContext(),
            base.Owner,
            base.DynamicVars["StrengthGain"].BaseValue,
            base.Owner,
            null
        );

        _triggeredThisTurn = true;
    }

    private int GetDisplayValue()
    {
        if(_triggeredThisTurn) {return 0;}

        int max = Math.Max(1, base.Amount);
        return Math.Clamp((int)_Data.damageThreshold - _Data.damageDealtThisTurn, 1, max);
    }
}
