using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers.ReusablePowers;


namespace TheScaled.TheScaledCode.Cards;
  
  
public class Snare : TheScaledCard
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("DrownAmount", 5m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];

    public Snare() : base(1, CardType.Attack, CardRarity.Common,TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this,cardPlay).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
        
        await PowerCmd.Apply<DrownedPower>(choiceContext, cardPlay.Target, base.DynamicVars["DrownAmount"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
	{
        base.EnergyCost.UpgradeBy(-1);
	}

    
}