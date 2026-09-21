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
    public override string CustomPortraitPath => "res://TheScaled/images/card_portraits/big/mud.png";
    public override int MaxUpgradeLevel => 0;
    public override bool HasTurnEndInHandEffect => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Ethereal];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        HoverTipFactory.FromAffliction<Muddied>();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("DexterityLoss", 1)];

    public Mud()
        : base(1, CardType.Status, CardRarity.Status, TargetType.None) { }

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        return Task.CompletedTask;
    }

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {

        var pile = PileType.Hand.GetPile(base.Owner);
        
        await MuddyCards(pile,1,base.Owner);

        

    }

    /// <summary>
    /// Applies the Muddied enchantment to a number of cards in the specified pile, up to the specified amount.
    /// If not enough cards are available, it will apply the enchantment to as many as possible.
    /// </summary>
    /// <param name="pile"></param>
    /// <param name="amount"></param>
    /// <param name="owner"></param>
    /// <returns></returns>
    public static async Task MuddyCards(CardPile pile, int amount, Player owner, bool skipVisuals = false)
    {
        AfflictionModel muddied = ModelDb.Affliction<Muddied>();
        var cardsAffected = new List<CardModel>();
        for (int i = 0; i < amount; i++)
        {
            CardModel? cardModel = owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards.Where(muddied.CanAfflict));

            if (cardModel != null)
            {
                await CardCmd.Afflict<Muddied>(cardModel, 1);
                cardsAffected.Add(cardModel);
            }
        }


        if(!skipVisuals) {CardCmd.Preview(cardsAffected,0.8f);}
        
    }

    /// <summary>
    /// Adds 'amount' Mud card(s) to the specified pile for the given player.
    /// </summary>
    /// <param name="pile"></param>
    /// <param name="amount"></param>
    /// <param name="owner"></param>
    /// <returns></returns>
    public static async Task AddMudCard(PileType pile, int amount, Player owner)
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
        CardCmd.Preview(list, 0.4f);
    }
}
