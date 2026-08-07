using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

public class Respite : TheScaledCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("PlayerExertionLoss", 3m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ExertionPower>()];


    public Respite()
        : base(0, CardType.Skill, CardRarity.Basic, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<TemporaryExertionDownPower>(
            choiceContext,
            base.Owner.Creature,
            base.DynamicVars["PlayerExertionLoss"].BaseValue,
            base.Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["PlayerExertionLoss"].UpgradeValueBy(1m);
    }
}
