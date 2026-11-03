using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Potions;

  

public class JarOfDirt : TheScaledPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

	public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Mud>(upgrade: true)];

	protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
	{
		AssertValidForTargetedPotion(target);
		await Mud.AddMudCard(PileType.Hand, base.DynamicVars.Cards.IntValue, target.Player!, upgraded: true, skipVisuals: true);
		
	}
}