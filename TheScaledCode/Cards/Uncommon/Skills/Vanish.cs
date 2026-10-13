using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class Vanish : TheScaledCard
{
    public Vanish()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    protected override bool ShouldGlowGoldInternal => base.CombatState?.HittableEnemies.Any((Creature e) => e.HasPower<DrownedPower>()) ?? false;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<DrownedPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        var drownPower = cardPlay.Target.GetPower<DrownedPower>();
        if (drownPower is not null)
        {
            var blockAmount = IsUpgraded ? drownPower.Amount * 2 : drownPower.Amount;
            await PowerCmd.Remove(drownPower);
            await CreatureCmd.GainBlock(cardPlay.Target, blockAmount, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move,cardPlay);
        }
    }

}
