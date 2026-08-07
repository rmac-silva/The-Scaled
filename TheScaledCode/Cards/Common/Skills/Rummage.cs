using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Enchantments;

namespace TheScaled.TheScaledCode.Cards;

  
public class Rummage : TheScaledCard
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(2, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromEnchantment<Muddied>().First()];
    public Rummage() : base(1, CardType.Skill, CardRarity.Common, TargetType.None)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> hand = PileType.Hand.GetPile(base.Owner).Cards.ToList();
        int affectedCards = 0;
        foreach (var card in hand)
        {
            if (card.Enchantment != null && card.Enchantment is Muddied)
            {
                card.ClearEnchantmentInternal();
                affectedCards++;
            }
        }
        
        //Gain block equal to 2 times the number of cards that were affected
        await CreatureCmd.GainBlock(base.Owner.Creature, (decimal)(DynamicVars.Block.IntValue * affectedCards), MegaCrit.Sts2.Core.ValueProps.ValueProp.Move, cardPlay);

    }
}