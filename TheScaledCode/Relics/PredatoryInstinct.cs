using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Relics;

  
public class PredatoryInstinct : TheScaledRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("EnemyAmbushGain", 3m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>(base.DynamicVars["EnemyAmbushGain"].IntValue)];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if(room is CombatRoom)
        {
            ModLog.Info(this,$"Adding {base.DynamicVars["EnemyAmbushGain"].IntValue} Ambush to enemies!");
            var hittableEnemies = ((CombatRoom)room).Enemies;
            
            await PowerCmd.Apply<Ambush>(new ThrowingPlayerChoiceContext(), hittableEnemies, base.DynamicVars["EnemyAmbushGain"].BaseValue, base.Owner.Creature,null);
            
        }
    }
}