using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;
  
public class DragUnder : TheScaledCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrownedPower>(3)];
    
    public DragUnder() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }
    protected override bool ShouldGlowGoldInternal => Owner.HasPower<DrownedPower>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var drownPower = Owner.Creature.GetPower<DrownedPower>();
        if(drownPower is not null)
        {
            var amountToApply =  drownPower.Amount * base.DynamicVars["DrownedPower"].IntValue;
            await PowerCmd.Remove<DrownedPower>(Owner.Creature);
            await PowerCmd.Apply<DrownedPower>(choiceContext,cardPlay.Target!,amountToApply,Owner.Creature,this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["DrownedPower"].UpgradeValueBy(2);
    }

}