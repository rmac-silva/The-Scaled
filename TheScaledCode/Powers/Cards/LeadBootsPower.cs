using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using TheScaled.TheScaledCode.Cards;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Powers;



  
  
public class LeadBootsPower : TheScaledPower
{

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrownedPower>(0)];


    public override Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if(target == base.Owner && amount > 0)
        {
            base.DynamicVars["DrownedPower"].BaseValue += 3*amount;
        }
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnStartLate(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if(side == CombatSide.Player && Owner.IsPlayer)
        {
            await PowerCmd.Apply<DrownedPower>(new ThrowingPlayerChoiceContext(), base.Owner,DynamicVars["DrownedPower"].IntValue * Amount, base.Owner, null);
        
            IEnumerable<CardModel> cards = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, Amount), context: new ThrowingPlayerChoiceContext(), player: base.Owner.Player!, filter: (CardModel c) => !c.Keywords.Contains(CardKeyword.Retain), source: this);
            if (cards != null)
            {
                foreach (CardModel card in cards)
                {
                    CardCmd.ApplyKeyword(card, CardKeyword.Retain);
                }
            }
        }
        return;
    }

}
