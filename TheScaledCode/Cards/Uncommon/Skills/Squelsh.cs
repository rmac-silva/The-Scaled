using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;

  
public class Squelsh : TheScaledCard
{
    public Squelsh()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            return CardPile
                    .Get(PileType.Hand, Owner)
                    ?.Cards.Any((CardModel c) => c is Mud)
                ?? false;
        }
    }


    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromAffliction<Muddied>().First()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = CardPile.Get(PileType.Hand,Owner)?.Cards.Where((c) => c is Mud && c.IsUpgradable).ToList();
        if (cards is null ||cards.Count == 0)
        {
            return;
        } else
        {
            foreach( CardModel c in cards)
            {
                CardCmd.Upgrade(c);
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.AddKeyword(CardKeyword.Retain);
    }
}
