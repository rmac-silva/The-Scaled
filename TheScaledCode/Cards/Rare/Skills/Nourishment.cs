using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;


public class Nourishment : SetupCard
{
    public Nourishment() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2)];

    protected override SetupCardType CardSetupType => SetupCardType.Debuff;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await AddSetup(cardPlay);
    }

    public override async Task AmbushEffect(AmbushMethodInfo info)
    {
        await PowerCmd.Apply<NourishmentPower>(info.choiceContext, info.target, DynamicVars["StrengthPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<NourishmentPower>(info.choiceContext, Owner.Creature, DynamicVars["StrengthPower"].IntValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        base.DynamicVars["StrengthPower"].UpgradeValueBy(1);
    }
}