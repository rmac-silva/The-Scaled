using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Potions;

  

public class PocketBush : TheScaledPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.AnyEnemy;
	public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>()];

	protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
	{
		if(target != null)
		{
			var amb = target.GetPower<Ambush>();
			if(amb != null)
			{
				await amb.TriggerAmbushExternal(choiceContext);
			} else
			{
				amb = await PowerCmd.Apply<Ambush>(choiceContext, target, 1, Owner.Creature, null);
				await amb!.TriggerAmbushExternal(choiceContext);
			}
		}
	}
}