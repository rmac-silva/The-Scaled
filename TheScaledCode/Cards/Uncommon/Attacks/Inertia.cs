using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class Inertia : SetupCard
{
    public Inertia() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }
    bool _applied = false;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered), new DynamicVar("DamageIncrease", 6m)];
    protected override SetupCardType CardSetupType => SetupCardType.Offensive;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await AddSetup(cardPlay);
        _applied = true;

    }

    public override async Task AmbushEffect(AmbushMethodInfo info)
    {
        await CreatureCmd.Damage(
            info.choiceContext,
            info.target,
            base.DynamicVars.Damage.IntValue,
            MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered,
            base.Owner.Creature
        );
        _applied = false;
        return;
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
       if(side == CombatSide.Player && _applied)
        {
            base.DynamicVars.Damage.BaseValue += base.DynamicVars["DamageIncrease"].IntValue;
        }
        return Task.CompletedTask;
    }

    protected override void AfterCloned()
    {
        base.AfterCloned();
        _applied = false;
    }



    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["DamageIncrease"].UpgradeValueBy(3);
    }
}
