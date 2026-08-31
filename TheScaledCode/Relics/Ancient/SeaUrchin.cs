using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using TheScaled.TheScaledCode.Character;

namespace TheScaled.TheScaledCode.Relics.Ancient;

[Pool(typeof(EventRelicPool))]
public class SeaUrchin : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private CardModel _exhaustedCard = null;

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        ArgumentNullException.ThrowIfNull(base.Owner.PlayerCombatState,"base.Owner.PlayerCombatState");
        
        if(base.Owner.PlayerCombatState.TurnNumber > 1)
        {
            return;
        }

        if(side != CombatSide.Player)
        {
            return;
        }

        //Turn 1, on the player's side

        //Exhaust a random unupgraded card
        var possibleCards = PileType.Deck.GetPile(base.Owner).Cards.Where( c => !c.IsUpgraded && c.IsUpgradable);

        var chosenCard = base.Owner.PlayerRng.Rewards.NextItem(possibleCards);

        if(chosenCard is null)
        {
            _exhaustedCard = null;
            return;
        }

        _exhaustedCard = chosenCard;

        var equivalentCombatCard = base.Owner.PlayerCombatState.DrawPile.Cards.First(c => c.CanonicalInstance == chosenCard.CanonicalInstance);

        await CardCmd.Exhaust(choiceContext,equivalentCombatCard);
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        if(_exhaustedCard is not null)
        {
            Flash();
            ModLog.Info(this,$"Finished combat. Looking for {_exhaustedCard.Id} ({_exhaustedCard.CanonicalInstance}) in player deck!");
            var c = PileType.Deck.GetPile(base.Owner).Cards.First((CardModel c) => c == _exhaustedCard);

            CardCmd.Upgrade(c);

            _exhaustedCard = null;
        }

        return Task.CompletedTask;
    }
}