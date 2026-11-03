using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;
      
  
public class Bide : TheScaledCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [new CardsVar(2),new PowerVar<Ambush>(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>()];

    public Bide() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        
        await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext,Owner.Creature,DynamicVars.Cards.IntValue,Owner.Creature,this);
        await PowerCmd.Apply<Ambush>(choiceContext, cardPlay.Target, DynamicVars["Ambush"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1);
        base.DynamicVars["Ambush"].UpgradeValueBy(1);
    }

    
}
