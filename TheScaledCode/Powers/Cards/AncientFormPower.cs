using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Powers.Cards;



  
public class AncientFormPower : TheScaledPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        //Chose 1 card fom your draw pile and discard pile to put into your hand
        if (player == base.Owner.Player)
        {
            await CardPileCmd.ShuffleIfNecessary(choiceContext, base.Owner.Player);

            //Merge draw and discard pile
            var drawPileCards = CardPile.Get(PileType.Draw,player)?.Cards;
            var discardPileCards = CardPile.Get(PileType.Discard,player)?.Cards;
            if(drawPileCards is null || discardPileCards is null)
            {
                ModLog.Error(this,$"Draw or Discard piles are null! Which should never happen.",new NullReferenceException());
                return;
            }

            var mergedCards = drawPileCards.Concat(discardPileCards).ToList().AsReadOnly();

            await CardPileCmd.Add(await CardSelectCmd.FromSimpleGrid(choiceContext, mergedCards, base.Owner.Player, new CardSelectorPrefs(base.SelectionScreenPrompt, base.Amount * base.DynamicVars.Cards.IntValue)), PileType.Hand);
        }

    }

    

}
