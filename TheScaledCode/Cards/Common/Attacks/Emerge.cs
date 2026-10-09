using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;

  
[Pool(typeof(TokenCardPool))]
public class Emerge : CustomCardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
        ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Submerge>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public Emerge()
        : base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await CardPileCmd.AddGeneratedCardToCombat(ModelDb.Card<Submerge>(), PileType.Draw, Owner);
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if(card == this)
        {
            AfflictInternal(ModelDb.Affliction<Muddied>(), 1);
        }

        return Task.CompletedTask;
    }


    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
    }

    
}
