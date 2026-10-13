using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;



  
  
public class Sink : TheScaledCard
{
    public Sink()
        : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            return Mud.HasMuddied(this);
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromAffliction<Muddied>().First()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature,base.DynamicVars.Block,cardPlay);
        if(Mud.HasMuddied(this))
        {
            await PowerCmd.Apply<BlurPower>(choiceContext,Owner.Creature,1,Owner.Creature,this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(6);
    }

    
}
