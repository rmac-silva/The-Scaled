using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;

public class Rinse : TheScaledCard
{
    public Rinse()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            return CardPile
                    .Get(PileType.Hand, Owner)
                    ?.Cards.Any((CardModel c) => c.Affliction != null && c.Affliction is Muddied)
                ?? false;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromAffliction<Muddied>().First()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = await CardSelectCmd.FromHand(
            choiceContext,
            Owner,
            new CardSelectorPrefs(
                CardSelectorPrefs.ExhaustSelectionPrompt,
                0,
                base.DynamicVars.Cards.IntValue
            ),
            HasMuddied,
            this
        );

        if (cards is null || cards.Count() == 0)
        {
            return;
        }

        foreach (CardModel c in cards)
        {
            await CardCmd.Exhaust(choiceContext, c);
            await CardPileCmd.Draw(choiceContext, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1);
    }

    private bool HasMuddied(CardModel c)
    {
        return c.Affliction != null && c.Affliction is Muddied;
    }
}
