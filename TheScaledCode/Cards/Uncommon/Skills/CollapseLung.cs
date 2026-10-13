using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;
  
  
public class CollapseLung : TheScaledCard
{
    
    public CollapseLung() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }
    protected override bool ShouldGlowGoldInternal => base.CombatState!.Enemies.Any(x => x.HasPower<DrownedPower>());

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var drownPower = cardPlay.Target!.GetPower<DrownedPower>();
        if(drownPower is not null)
        {
            await PowerCmd.Apply<DrownedPower>(choiceContext, cardPlay.Target, drownPower.Amount, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }

}