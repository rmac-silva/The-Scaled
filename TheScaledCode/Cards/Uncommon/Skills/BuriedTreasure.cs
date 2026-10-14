using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;



  
public class BuriedTreasure : TheScaledCard
{
    public BuriedTreasure()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            return CardPile
                    .Get(PileType.Draw, Owner)
                    ?.Cards.Any(Mud.HasMuddied)
                ?? false;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromAffliction<Muddied>().First()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> allCards = new List<CardModel>();
        List<CardModel> drawPile = PileType.Draw.GetPile(base.Owner).Cards.Where(Mud.HasMuddied).ToList();

        if(IsUpgraded)
        {
            List<CardModel> discardPile = PileType.Discard.GetPile(base.Owner).Cards.Where(Mud.HasMuddied).ToList();
            List<CardModel> exhaustPile = PileType.Exhaust.GetPile(base.Owner).Cards.Where(Mud.HasMuddied).ToList();
            allCards = drawPile.Union(discardPile.Union(exhaustPile)).ToList();
        } else
        {
            allCards = drawPile;
        }

        var cards = await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            allCards,
            Owner,
            new CardSelectorPrefs(
                SelectionScreenPrompt,
                DynamicVars.Cards.IntValue,
                DynamicVars.Cards.IntValue
            )
        );

        if (cards is null || cards.Count() == 0)
        {
            return;
        }
        foreach (CardModel card in cards)
        {
            await CardPileCmd.Add(card, PileType.Hand);
        }
        
    }

    protected override void OnUpgrade()
    {
    }

    
}
