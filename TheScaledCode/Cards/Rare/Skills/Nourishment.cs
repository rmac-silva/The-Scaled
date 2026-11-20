using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;


public class Nourishment : SetupCard
{
    public Nourishment() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<NourishmentSetup>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [AmbushHoverTip];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<NourishmentSetup>(choiceContext, cardPlay.Target!, DynamicVars["NourishmentSetup"].IntValue, Owner.Creature, this);
    }

    
    protected override void OnUpgrade()
    {
        DynamicVars["NourishmentSetup"].UpgradeValueBy(1);
    }
}