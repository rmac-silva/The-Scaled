using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class BlindFlail : TheScaledCard
{
    public BlindFlail()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

    


    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromAffliction<Muddied>().First()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = CardPile.Get(PileType.Draw,Owner)?.Cards.Where((c) => c is Mud && c.IsUpgradable).ToList();
        if (cards is null ||cards.Count == 0)
        {
            return;
        } else
        {
            foreach( CardModel c in cards)
            {
                await CardCmd.AutoPlay(choiceContext, c, null);
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1);
    }

}
