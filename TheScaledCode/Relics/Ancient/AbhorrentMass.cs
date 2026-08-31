using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using TheScaled.TheScaledCode.Cards;
using TheScaled.TheScaledCode.Character;

namespace TheScaled.TheScaledCode.Relics.Ancient;

[Pool(typeof(EventRelicPool))]
public class AbhorrentMass : CustomRelicModel
{

    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Abyss>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("AbyssCards",3), new DynamicVar("Relics",3)];

    public override async Task AfterObtained()
    {
        //Add X act 3 ancient relics, excluding ourselves
        List<EventOption> possibleRelics =
        [
            .. ModelDb.AncientEvent<Nonupeipe>().AllPossibleOptions,
            .. ModelDb.AncientEvent<Tanx>().AllPossibleOptions,
            .. ModelDb.AncientEvent<Vakuu>().AllPossibleOptions,
            .. ModelDb.AncientEvent<Darv>().AllPossibleOptions,
        ];
        
        List<Reward> relicsOffered = new List<Reward>();

        for(int i = 0; i < base.DynamicVars["Relics"].IntValue; i++)
        {
            Rng rng = base.Owner.RunState.Rng.UpFront;
            var r = rng.NextItem(possibleRelics);
            relicsOffered.Add(new RelicReward(r.Relic, base.Owner));
        }

        await new RewardsSet(base.Owner).WithCustomRewards(relicsOffered).WithSkippingDisallowed().Offer();

        //Add X abyss cards
        List<CardPileAddResult> addedCards = new List<CardPileAddResult>(base.DynamicVars["AbyssCards"].IntValue);

        for(int i = 0; i < base.DynamicVars["AbyssCards"].IntValue; i++)
        {
            var card = base.Owner.RunState.CreateCard<Abyss>(base.Owner);
            addedCards.Add(await CardPileCmd.Add(card,PileType.Deck));
        }

        CardCmd.PreviewCardPileAdd(addedCards, 2f);
        await Cmd.Wait(0.5f);
    }

    
}