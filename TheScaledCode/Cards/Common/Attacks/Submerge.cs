using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class Submerge : TheScaledCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(8m,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)
        ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Emerge>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public Submerge()
        : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block,cardPlay);

        CardModel cardModel = base.CombatState.CreateCard<Emerge>(base.Owner);
        await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Draw, Owner); 
    }


    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }

    
}
