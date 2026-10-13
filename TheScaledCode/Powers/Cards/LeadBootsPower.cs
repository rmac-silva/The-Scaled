using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Powers;



  
  
public class LeadBootsPower : TheScaledPower
{

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrownedPower>(0)];


    

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if(power == this && amount > 0)
        {
            base.DynamicVars["DrownedPower"].BaseValue = Amount * 3;
        }
        return Task.CompletedTask;
    }

    

    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        if(player != Owner.Player)
        {
            return;
        }

        await PowerCmd.Apply<DrownedPower>(choiceContext, base.Owner, Amount * 3, base.Owner, null);
        
        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, Amount);
		List<CardModel> cards = (await CardSelectCmd.FromHand(choiceContext, player, prefs, NoRetain, this)).ToList();
        if (cards != null)
        {
            foreach (CardModel card in cards)
            {
                CardCmd.ApplyKeyword(card, CardKeyword.Retain);
            }
        }
    }

    private bool NoRetain(CardModel card)
    {
        return !card.Keywords.Contains(CardKeyword.Retain);
    }


}
