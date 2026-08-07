using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace TheScaled.TheScaledCode.Helpers
{
    
public static class IntentHelper
{
    public static bool IntendsToBlock(MoveState nextMove)
    {
        nextMove.Intents.Any(delegate(AbstractIntent intent)
        {
            IntentType intentType = intent.IntentType;
            return intentType == IntentType.Defend ? true : false;
        });

        return false;
    }
}
}