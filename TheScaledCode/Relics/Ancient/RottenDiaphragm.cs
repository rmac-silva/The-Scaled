using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using TheScaled.TheScaledCode.Character;

namespace TheScaled.TheScaledCode.Relics.Ancient;

[Pool(typeof(EventRelicPool))]
public class RottenDiaphragm : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override bool IsUsedUp => _triggeredThisTurn;
    private bool _triggeredThisTurn = false;

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        ArgumentNullException.ThrowIfNull(Owner.PlayerCombatState,"Owner.PlayerCombatState");

        //If it's not our card we ignore it
        if (card.Owner != base.Owner)
        {
            return;
        }

        if (_triggeredThisTurn)
        {
            return;
        }

        if(causedByEthereal)
        {
            return;
        }
        
        await CardPileCmd.Add(card,PileType.Discard);


        _triggeredThisTurn = true;
        InvokeDisplayAmountChanged();
    }

    public override Task AfterModifyingCardPlayResultPileOrPosition(
        CardModel card,
        PileType pileType,
        CardPilePosition position
    )
    {
        if (card.Owner != base.Owner)
        {
            return Task.CompletedTask;
        }
        Flash();
        return Task.CompletedTask;
    }

    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState
    )
    {
        if (side == CombatSide.Player)
        {
            _triggeredThisTurn = false;
        }

        return Task.CompletedTask;
    }

    public override CardCreationOptions ModifyCardRewardCreationOptions(
        Player player,
        CardCreationOptions options
    )
    {
        if (base.Owner != player)
        {
            return options;
        }
        if (options.Flags.HasFlag(CardCreationFlags.NoCardPoolModifications))
        {
            return options;
        }
        if (!options.Flags.HasFlag(CardCreationFlags.IsCardReward))
        {
            return options;
        }
        if (options.CustomCardPool != null)
        {
            return options;
        }
        if (options.CardPools.All((CardPoolModel p) => p.IsColorless))
        {
            return options;
        }

        //If we have already generated a curse, don't add our 'curse' pool
        if (CardRewardWithCursesPatch.GeneratedCurseThisCardReward)
        {
            return options;
        }

        IEnumerable<CardPoolModel> pools = [ModelDb.CardPool<CurseCardPool>(), base.Owner.Character.CardPool];
        var o = new CardCreationOptions(pools,options.Source,CardRarityOddsType.Uniform);
        return o;
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        CardRewardWithCursesPatch.ResetFlag();
        return Task.CompletedTask;
    }
}
