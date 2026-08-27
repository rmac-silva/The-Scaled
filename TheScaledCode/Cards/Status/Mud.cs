using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Enchantments;

namespace TheScaled.TheScaledCode.Cards;

public class Mud : TheScaledCard
{
    public override int MaxUpgradeLevel => 0;
    public override bool HasTurnEndInHandEffect => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Ethereal];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        HoverTipFactory.FromEnchantment<Muddied>();

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
    public static async Task MuddyCards(CardPile pile, int amount, Player owner)
    {
        EnchantmentModel muddied = ModelDb.Enchantment<Muddied>();

        for (int i = 0; i < amount; i++)
        {
            CardModel? cardModel = owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards.Where(muddied.CanEnchant));

            if (cardModel != null)
            {
                CardCmd.Enchant<Muddied>(cardModel, 1);
            }
        }
        
    }
}
