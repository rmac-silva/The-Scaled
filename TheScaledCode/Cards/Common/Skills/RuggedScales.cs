using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class RuggedScales : SetupCard
{
    public RuggedScales() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("AmbushEffect", 9)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ThornsPower>()];

    protected override SetupCardType CardSetupType => throw new NotImplementedException();

    public override async Task AmbushEffect(AmbushMethodInfo info, Dictionary<string, decimal> data)
    {
        if(info.applier is null)
        {
            ModLog.Warning(this,$"Ambush effect applier was null, which shouldn't happen!");
            return;
        }
        
        await CreatureCmd.GainBlock(info.applier,base.DynamicVars["AmbushEffect"].IntValue,MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered,null);
    }


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3);
    }
}