using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class Concussion : SetupCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [AmbushHoverTip, StunIntent.GetStaticHoverTip()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override SetupCardType CardSetupType => SetupCardType.Stun;

    public Concussion() : base(3, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await AddSetup(cardPlay);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override async Task AmbushEffect(AmbushMethodInfo info, Dictionary<string, decimal> _)
    {
        if(info.target is null)
        {
            ModLog.Warning(this,"AmbushEffect called with null target");
            return;
        }

        await CreatureCmd.Stun(info.target);

        return;
    }
}