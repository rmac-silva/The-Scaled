using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Powers;

public class BellyFlopSetup : BaseSetupPower
{
    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        CardModel? cardModel = PileType.Draw.GetPile(Applier!.Player!).Cards.Where((CardModel c) => !c.Keywords.Contains(CardKeyword.Unplayable)).ToList().StableShuffle(Applier!.Player!.RunState.Rng.Shuffle)
				.FirstOrDefault();
			if (cardModel == null)
			{
				cardModel = PileType.Draw.GetPile(Applier.Player).Cards.ToList().StableShuffle(Applier.Player.RunState.Rng.Shuffle).FirstOrDefault();
			}
			if (cardModel != null)
			{
				await CardCmd.AutoPlay(ctx, cardModel, null);
			}
    }
}