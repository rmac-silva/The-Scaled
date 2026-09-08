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


public class PrismaticScalesPower : TheScaledPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<DrownedPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrownedPower>(1)];

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (!(amount <= 0m) && creature == base.Owner)
        {
            if (!(amount <= 0m) && creature == base.Owner)
            {
                IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;

                await PowerCmd.Apply<DrownedPower>(new ThrowingPlayerChoiceContext(), hittableEnemies, base.Amount, base.Owner, null);
            }
        }
    }

}
