using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;

[Pool(typeof(StatusCardPool))]
public class Mud : CustomCardModel
{
    public override string CustomPortraitPath =>
        "res://TheScaled/images/card_portraits/big/mud.png";
    public override int MaxUpgradeLevel => 1;
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        HoverTipFactory.FromAffliction<Muddied>();

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1)
    ];


    public Mud()
        : base(1, CardType.Status, CardRarity.Status, TargetType.None) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, Owner);
        await MuddyCards(cards.ToList());
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }


    /// <summary>
    /// Applies the Muddied affliction to the provided list of cards. If the list is null or empty, logs an error and returns.
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="skipVisuals"></param>
    public static async Task<IEnumerable<Muddied>> MuddyCards(
        List<CardModel> cards)
    {
        if(cards == null || cards.Count == 0)
        {
            ModLog.Error(null, "No cards provided to muddy.", new ArgumentNullException());
            return Enumerable.Empty<Muddied>();
        }

        
            return await CardCmd.AfflictAndPreview<Muddied>(cards, 1);
        
    }

    /// <summary>
    /// Applies the Muddied affliction to the provided list of cards without showing any visual effects. If the list is null or empty, logs an error and returns.
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static async Task<List<CardModel>> MuddyCardsNoVisuals(
        List<CardModel> cards)
    {
        if(cards == null || cards.Count == 0)
        {
            ModLog.Error(null, "No cards provided to muddy.", new ArgumentNullException());
            return new List<CardModel>();
        }

        
            foreach (var card in cards)
            {
                if (Muddied.CanAfflictMuddied(card))
                {
                    await CardCmd.Afflict<Muddied>(card, 1);
                }
            }

            return cards;
        
    }

    /// <summary>
    /// Adds 'amount' Mud card(s) to the specified pile for the given player.
    /// </summary>
    /// <param name="pile"></param>
    /// <param name="amount"></param>
    /// <param name="owner"></param>
    /// <param name="skipVisuals"></param>
    /// <returns></returns>
    public static async Task AddMudCard(PileType pile, int amount, Player owner, bool skipVisuals = false)
    {
        ArgumentNullException.ThrowIfNull(owner.Creature.CombatState, "owner.Creature.CombatState");

        List<CardModel> list = new List<CardModel>();
        for (int i = 0; i < amount; i++)
        {
            CardModel card = owner.Creature.CombatState.CreateCard<Mud>(owner);
            list.Add(card);
        }
        //Add the cards to the discard pile
        await CardPileCmd.AddGeneratedCardsToCombat(list, pile, owner);

        if (!skipVisuals)
        {
            CardCmd.Preview(list, 0.4f);
        }
        
        CardPile.Get(PileType.Draw,owner)?.InvokeContentsChanged();
    }
}
