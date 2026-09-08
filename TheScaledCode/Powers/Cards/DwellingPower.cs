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


public class DwellingPower : TheScaledPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Ambush>()];

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if(side == CombatSide.Player)
        {
            var totalAmbush = 0;
            foreach( Creature e in base.CombatState.HittableEnemies)
            {
                totalAmbush += e.GetPower<Ambush>()?.Amount ?? 0;
            }

            CreatureCmd.GainBlock(base.Owner, totalAmbush * base.Amount, ValueProp.Unpowered, null);
        }

        return Task.CompletedTask;

    }

}
