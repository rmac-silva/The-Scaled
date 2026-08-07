using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers
{
    public class MudPower : TheScaledPower, ITemporaryPower
    {
        private bool _shouldIgnoreNextInstance;
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public AbstractModel OriginModel => ModelDb.Card<Mud>();

        public PowerModel InternallyAppliedPower => ModelDb.Power<ExertionPower>();

        protected virtual bool IsPositive => false;

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

        public void IgnoreNextInstance()
        {
            throw new NotImplementedException();
        }

        public override async Task BeforeApplied(
            Creature target,
            decimal amount,
            Creature? applier,
            CardModel? cardSource
        )
        {
            if (_shouldIgnoreNextInstance)
            {
                _shouldIgnoreNextInstance = false;
            }
            else
            {
                await PowerCmd.Apply<DexterityPower>(
                    new ThrowingPlayerChoiceContext(),
                    target,
                    (decimal)Sign * amount,
                    applier,
                    cardSource,
                    silent: true
                );
            }
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
                if (_shouldIgnoreNextInstance)
                {
                    _shouldIgnoreNextInstance = false;
                }
                else
                {
                    await PowerCmd.Apply<DexterityPower>(
                        choiceContext,
                        base.Owner,
                        (decimal)Sign * amount,
                        applier,
                        cardSource,
                        silent: true
                    );
                }
            }
        }

        public override async Task AfterSideTurnEnd(
            PlayerChoiceContext choiceContext,
            CombatSide side,
            IEnumerable<Creature> participants
        )
        {
            if (SkipNextDurationTick)
            {
                SkipNextDurationTick = false;
                return;
            }

            if (side == CombatSide.Player && participants.Contains(base.Owner))
            {
                Flash();
                await PowerCmd.Remove(this);
                await PowerCmd.Apply<DexterityPower>(
                    choiceContext,
                    base.Owner,
                    -Sign * base.Amount,
                    base.Owner,
                    null
                );
            }
        }

        public void SkipNextTick()
        {
            SkipNextDurationTick = true;
        }
    }
}
