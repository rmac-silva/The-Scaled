using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

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
                    .Get(PileType.Draw, Owner)
                    ?.Cards.Any((CardModel c) => c is Mud)
                ?? false;
        }
    }


    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<Mud>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = CardPile.Get(PileType.Draw,Owner)?.Cards.Where((c) => c is Mud && c.IsUpgradable).TakeRandom(base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardSelection).ToList();
        if (cards is null || cards.Count == 0)
        {
            return;
        } else
        {
            foreach( CardModel c in cards)
            {
                CardCmd.Upgrade(c);
                await CardPileCmd.Add(c, PileType.Hand);
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1);
    }
}
