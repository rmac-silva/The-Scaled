using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheScaled.TheScaledCode.Powers;


  
public class SuffocatePower : TheScaledPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if(player == base.Owner.Player)
        {
            await PowerCmd.Apply<Ambush>(choiceContext, player.Creature!.CombatState!.Enemies, Amount, player.Creature, null);
        }
        return;
    }
}