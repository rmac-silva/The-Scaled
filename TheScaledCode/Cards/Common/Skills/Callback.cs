using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class Callback : TheScaledCard
{
    public Callback() : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Ambush>(2), new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>(),HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<Ambush>(choiceContext, cardPlay.Target!, DynamicVars["Ambush"].IntValue, Owner.Creature, this);
        var cards = PileType.Draw.GetPile(Owner).Cards.Where((CardModel c) => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList();
        CardModel? cardModel = Owner.RunState.Rng.Shuffle.NextItem(cards);
			if (cardModel != null)
			{
				await CardCmd.AutoPlay(choiceContext, cardModel, null);
			}
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}