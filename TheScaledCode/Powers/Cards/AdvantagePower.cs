using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers.Cards;


  
public class AdvantagePower : TheScaledPower
{
    private bool _hasTriggeredThisTurn = false;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SetupPower>()];
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if(cardPlay.Player.Creature != base.Owner)
        {
            return;
        }

        if(cardPlay.Target == null)
        {
            return;
        }

        if (cardPlay.Card is not SetupCard setupCard)
        {
            return;
        }

        if(_hasTriggeredThisTurn)
        {
            return;
        }

        var ambPwr = cardPlay.Target.GetPower<Ambush>();

        if(ambPwr != null)
        {
            _hasTriggeredThisTurn = true;
            var ambEntry = new AmbushEntry(setupCard.AmbushEffect,setupCard,setupCard.SetupData);
            await ambPwr.AddAmbushEffect(ambEntry,setupCard.GetHovertip(cardPlay.Target));
            CardCmd.Preview(cardPlay.Card,1f,CardPreviewStyle.MessyLayout);
        }
    }


    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if(side == CombatSide.Player)
        {
            _hasTriggeredThisTurn = false;
        }
        return Task.CompletedTask;
    }

}
