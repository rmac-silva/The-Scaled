using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;
public class HuntersMark : SetupCard
{
    public HuntersMark() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<HuntersMarkPower>(1)
        ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [AmbushHoverTip];

    protected override SetupCardType CardSetupType => SetupCardType.Buff;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await base.AddSetup(cardPlay);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }

    public override async Task AmbushEffect(AmbushMethodInfo info)
    {

        await PowerCmd.Apply<HuntersMarkPower>(info.choiceContext,info.target,base.DynamicVars["HuntersMarkPower"].IntValue,base.Owner.Creature,this);
        return;
    }
}