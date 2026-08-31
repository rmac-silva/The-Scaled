using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheScaled.TheScaledCode.Cards;

  
public class FrenziedBites : TheScaledCard
{
    public FrenziedBites() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("NumberHits",5)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
        .WithHitCount(base.DynamicVars["NumberHits"].IntValue)
        .FromCard(this)
        .TargetingRandomOpponents(base.CombatState)
        .WithHitFx("vfx/vfx_attack_slash")
        .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["NumberHits"].UpgradeValueBy(1);
    }
}