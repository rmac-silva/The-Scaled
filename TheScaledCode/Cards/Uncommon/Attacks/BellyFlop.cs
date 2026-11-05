using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class BellyFlop : SetupCard
{
    public BellyFlop() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10,ValueProp.Move), new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [AmbushHoverTip];
    protected override SetupCardType CardSetupType => SetupCardType.Mystery;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, "CombatState");
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
        .FromCard(this, cardPlay)
        .TargetingAllOpponents(CombatState)
        .Execute(choiceContext);

        await AddSetupToAllEnemies();
        
    }
    public override async Task AmbushEffect(AmbushMethodInfo info)
    {
        CardModel? cardModel = PileType.Draw.GetPile(base.Owner).Cards.Where((CardModel c) => !c.Keywords.Contains(CardKeyword.Unplayable)).ToList().StableShuffle(base.Owner.RunState.Rng.Shuffle)
				.FirstOrDefault();
			if (cardModel == null)
			{
				cardModel = PileType.Draw.GetPile(base.Owner).Cards.ToList().StableShuffle(base.Owner.RunState.Rng.Shuffle).FirstOrDefault();
			}
			if (cardModel != null)
			{
				await CardCmd.AutoPlay(info.choiceContext, cardModel, null);
			}
    }
}