using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Relics;

  
public class NaturalInstinct : TheScaledRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("PlayerExertionGain", 15m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ExertionPower>(base.DynamicVars["PlayerExertionGain"].IntValue)];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if(room is CombatRoom)
        {
            ModLog.Info(this,$"Adding {base.DynamicVars["PlayerExertionGain"].IntValue} Exertion to player!");
            await PowerCmd.Apply<ExertionPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["PlayerExertionGain"].BaseValue, base.Owner.Creature,null);
        }
    }
}