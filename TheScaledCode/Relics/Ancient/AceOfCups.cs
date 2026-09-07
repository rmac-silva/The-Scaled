using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace TheScaled.TheScaledCode.Relics.Ancient;

[Pool(typeof(EventRelicPool))]
public class AceOfCups : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override bool ShowCounter => true;
    public override int DisplayAmount
	{
		get
		{
			if (ShouldTrigger)
			{
				return 1;
			}
			return 0;
		}
	}

    private bool ShouldTrigger
    {
        get
        {
            return _shouldTrigger;
        }
        set
        {
            _shouldTrigger = value;
            InvokeDisplayAmountChanged();
        }
    }
    private bool _shouldTrigger = false;

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources, CardLocation cardLocation)
    {
        
        //If it's not our card we ignore it
        if (card.Owner != base.Owner)
		{
			return cardLocation;
		}

        //If it's not being moved to the discard pile
        if (cardLocation.pileType != PileType.Discard)
		{
			return cardLocation;
		}
        
        if(!ShouldTrigger)
        {
            ShouldTrigger = !ShouldTrigger;
            return cardLocation;
        }


        //Toggle the trigger
        ShouldTrigger = !ShouldTrigger;

        //Otherwise, it's our card, it should trigger and it's being moved to the discard pile
        //So we can shuffle it to the draw pile instead
        Flash();
        return new CardLocation(card.Owner,PileType.Draw, CardPilePosition.Random);
    }

    public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        ModLog.Info(this,$"Discarding {card}");

        if (card.Owner != base.Owner)
		{
			return;
		}

        if(!ShouldTrigger)
        {
            ShouldTrigger = !ShouldTrigger;
            return;
        }

         //Toggle the trigger
        ShouldTrigger = !ShouldTrigger;

        await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Random);
    }
    
}