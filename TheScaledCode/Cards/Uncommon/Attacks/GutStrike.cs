using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
 
public class GutStrike : TheScaledCard
{
    public GutStrike() : base(1,CardType.Attack,CardRarity.Uncommon,TargetType.AnyEnemy)
    {
    }
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> {CardTag.Strike};

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SetupPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0),
        new ExtraDamageVar(6),
        new CalculatedDamageVar(ValueProp.Move)
        .WithMultiplier((CardModel card, Creature? target) =>
            target is null
                ? 0m
                : target.Powers
            .OfType<Ambush>()
            .FirstOrDefault(power => power.Applier == card.Owner.Creature)?.NumSetupsForCreature ?? 0m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.ExtraDamage.UpgradeValueBy(2);
    }
}