using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Relics;

  

public class SticksAndStones : TheScaledRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(6,MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>()];

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if(IsCombatRoom(room))
        {
            EventManager.OnAmbushTriggered += OnAmbushTriggered;
        }
        return base.AfterRoomEntered(room);
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        EventManager.OnAmbushTriggered -= OnAmbushTriggered;
        return base.AfterCombatEnd(room);
    }


    private async void OnAmbushTriggered(Player p)
    {
        if(p != base.Owner.Creature.Player)
        {
            return;
        }

        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), p.Creature.CombatState!.HittableEnemies, base.DynamicVars.Damage, base.Owner.Creature);
        base.Flash();
    }

    private bool IsCombatRoom(AbstractRoom room)
    {
        return room.RoomType == RoomType.Monster || room.RoomType == RoomType.Elite || room.RoomType == RoomType.Boss;
    }

}