using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class BlindFlail : TheScaledCard
{
    public BlindFlail()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

    


    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromAffliction<Muddied>().First()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = CardPile.Get(PileType.Draw,Owner)?.Cards.ToList();

        

        for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            if(cards is null || cards.Count == 0)
            {
                return;
            }

            var card = cards?.FirstOrDefault((c) => c.Affliction != null && c.Affliction is Muddied);

            if (card is null)
            {
                return;
            } else
            {
                await CardCmd.AutoPlay(choiceContext, card, null);
                cards!.Remove(card);
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1);
    }

}
