using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;


namespace TheScaled.TheScaledCode.Cards;
  
  
  
public class InstinctiveStrike : TheScaledCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new PowerVar<Ambush>(2)];

    public InstinctiveStrike() : base(0, CardType.Attack, CardRarity.Common,TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this,cardPlay).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
        
        await PowerCmd.Apply<Ambush>(new ThrowingPlayerChoiceContext(), cardPlay.Target, base.DynamicVars["Ambush"].BaseValue, base.Owner.Creature, this);
        
    }

    protected override void OnUpgrade()
	{
		base.DynamicVars["Ambush"].UpgradeValueBy(1m);
	}

    
}