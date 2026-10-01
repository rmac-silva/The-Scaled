using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers.ReusablePowers;


namespace TheScaled.TheScaledCode.Cards;
  
  
public class Snare : TheScaledCard
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("DrownAmount", 2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];

    public Snare() : base(0, CardType.Attack, CardRarity.Common,TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        ArgumentNullException.ThrowIfNull(cardPlay.Target.Monster, "cardPlay.Target.Monster");

		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this,cardPlay).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);

        if (cardPlay.Target.Monster.IntendsToAttack)
		{
            await PowerCmd.Apply<DrownedPower>(choiceContext, cardPlay.Target, base.DynamicVars["DrownAmount"].BaseValue, base.Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
	{
        base.DynamicVars["DrownAmount"].UpgradeValueBy(1);
	}

    
}