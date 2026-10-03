using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Afflictions;
using TheScaled.TheScaledCode.Powers;


namespace TheScaled.TheScaledCode.Cards;
  
  
  
public class ViciousStrike : TheScaledCard
{
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> {CardTag.Strike};

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new PowerVar<ViciousStrikePower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ViciousStrikePower>()];
    public ViciousStrike() : base(0, CardType.Attack, CardRarity.Common,TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this,cardPlay).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);

        await PowerCmd.Apply<ViciousStrikePower>(choiceContext, base.Owner.Creature, base.DynamicVars["ViciousStrikePower"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
	{
        base.DynamicVars.Damage.UpgradeValueBy(2);
        base.DynamicVars["ViciousStrikePower"].UpgradeValueBy(1);
	}

    
}