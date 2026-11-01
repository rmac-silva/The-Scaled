using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;

  
public class Squelsh : TheScaledCard
{
    public Squelsh()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }



    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromAffliction<Muddied>().First(), HoverTipFactory.FromCard<Mud>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardsDrawn = await CardPileCmd.Draw(choiceContext,base.DynamicVars.Cards.IntValue, base.Owner);
        foreach (var card in cardsDrawn)
        {
            await CardCmd.Afflict<Muddied>(card,1);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1);
    }
}
