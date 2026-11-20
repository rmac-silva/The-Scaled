using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace TheScaled.TheScaledCode.Powers;





public class StunSetup : BaseSetupPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>(), StunIntent.GetStaticHoverTip()];

    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        await CreatureCmd.Stun(Owner);
    }
}