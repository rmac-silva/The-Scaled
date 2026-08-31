using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Enchantments;
using TheScaled.TheScaledCode.Helpers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

public class Mud : TheScaledCard
{
    public override int MaxUpgradeLevel => 0;
    public override bool HasTurnEndInHandEffect => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
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
        IEnumerable<CardModel> hand = PileType.Hand.GetPile(base.Owner).Cards.ToList();

        foreach (var card in hand)
        {
            if (EnchanteableHelper.CanBeEnchantedByMuddied(card))
            {
                CardCmd.Enchant<Muddied>(card, 1);
            }
        }

        //Lose one dexterity next turn
        (await PowerCmd.Apply<MudPower>(
            choiceContext,
            base.Owner.Creature,
            base.DynamicVars["DexterityLoss"].BaseValue,
            base.Owner.Creature,
            this
        ))?.SkipNextTick();
        
    }

    public static IEnumerable<Mud> Create(Player owner, int amount, ICombatState combatState)
    {
        List<Mud> list = new List<Mud>();
		for (int i = 0; i < amount; i++)
		{
			list.Add(combatState.CreateCard<Mud>(owner));
		}
		return list;
    }
}
