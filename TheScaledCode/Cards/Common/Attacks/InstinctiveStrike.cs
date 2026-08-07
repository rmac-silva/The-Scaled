using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;


namespace TheScaled.TheScaledCode.Cards;
  
  
  
public class InstinctiveStrike : TheScaledCard
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(11m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new CardsVar(1)];

    public InstinctiveStrike() : base(2, CardType.Attack, CardRarity.Common,TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
        
        await CardPileCmd.Draw(choiceContext,DynamicVars.Cards.BaseValue,base.Owner);
        
    }

    protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(4m);
	}

    
}