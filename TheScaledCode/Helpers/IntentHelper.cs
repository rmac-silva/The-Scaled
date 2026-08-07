using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace TheScaled.TheScaledCode.Helpers
{
    
public static class IntentHelper
{
    public static bool IntendsToBlock(MoveState nextMove)
    {
       return nextMove.Intents.Any(intent => intent.IntentType == IntentType.Defend);
    }
}
}