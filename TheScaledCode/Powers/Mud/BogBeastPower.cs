using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;


  
public class BogBeastPower : TheScaledPower
{
    public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Single;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromCard<Mud>(true)];

    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if(card is Mud && creator?.Creature == Owner && card.IsUpgradable)
        {
            CardCmd.Upgrade(card);
        }
        return Task.CompletedTask;
    }
}