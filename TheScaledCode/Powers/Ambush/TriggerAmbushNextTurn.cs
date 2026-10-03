using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers.Cards;


  
  
  
public class TriggerAmbushNextTurn : TheScaledPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Ambush>()];

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Single;


    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if(side == CombatSide.Player && base.Applier != null)
        {
            var Ambush = SetupCard.GetAmbushPowerForApplier(base.Applier,base.Owner);
            if(Ambush != null)
            {
                await Ambush.TriggerAmbushExternal();
            }
            RemoveInternal();
        }
        return;
    }

}
