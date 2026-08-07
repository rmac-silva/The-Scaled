using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class SmotheringPresence : TheScaledCard
{
    public SmotheringPresence() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<SmotheringPresencePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DrownReduction",2)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        (await PowerCmd.Apply<SmotheringPresencePower>(choiceContext,base.Owner.Creature,1,base.Owner.Creature,this))?.IncrementDrownReduction(base.DynamicVars["DrownReduction"].IntValue);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["DrownReduction"].UpgradeValueBy(-1);
    }
    

    
}