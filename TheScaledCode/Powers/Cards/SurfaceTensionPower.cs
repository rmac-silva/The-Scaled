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
using TheScaled.TheScaledCode.Cards;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Powers.Cards;


  
public class SurfaceTensionPower : TheScaledPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<DrownedPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DrownApplied", 1)];


    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if(target != Owner)
        {
            return;
        }

        //Took 0 damage, don't trigger the power
        if (result.Props.HasFlag(ValueProp.Unpowered))
        {
            return;
        }
        else
        {
            if(dealer is null)
            {
                return;
            }

            //Apply 1 Drowned to the enemy
            await PowerCmd.Apply<DrownedPower>(choiceContext,dealer,base.Amount * base.DynamicVars["DrownApplied"].IntValue,base.Owner,ModelDb.Card<SurfaceTension>());
        }
    }

    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if(side == CombatSide.Player)
        {
            base.SetAmount(0);
        }

        return Task.CompletedTask;
    }

}
