using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Cards;

  
public class HeadlongCharge : TheScaledCard
{
    public HeadlongCharge()
        : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(28m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new DynamicVar("CardsToExhaust", 1),
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        if (this.IsUpgraded)
        {
            //Exhaust selected card
            CardModel? cardModel = (
                await CardSelectCmd.FromHand(
                    prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1),
                    context: choiceContext,
                    player: base.Owner,
                    filter: null,
                    source: this
                )
            ).FirstOrDefault();
            if (cardModel != null)
            {
                await CardCmd.Exhaust(choiceContext, cardModel);
            }
            return;
        }
        else
        {
            //Exhaust random card
            CardPile pile = PileType.Hand.GetPile(base.Owner);
            CardModel? cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
            if (cardModel != null)
            {
                await CardCmd.Exhaust(choiceContext, cardModel);
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(8);
    }
}
