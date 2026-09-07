using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;


namespace TheScaled.TheScaledCode.Cards;
  
  
  
public class ViciousStrike : TheScaledCard
{
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> {CardTag.Strike};

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromAffliction<Muddied>().First()];
    public ViciousStrike() : base(0, CardType.Attack, CardRarity.Common,TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this,cardPlay).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if(card != this)
        {
            return;
        }

        if(this.Enchantment is not null && !this.Enchantment.IsStackable)
        {
            return;
        }

        await CardCmd.Afflict<Muddied>(this,1);
    }

    protected override void OnUpgrade()
	{
        base.DynamicVars.Damage.UpgradeValueBy(2);
	}

    
}