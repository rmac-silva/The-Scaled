using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;

  
public class FromTheDepths : TheScaledCard
{
    public FromTheDepths() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new RepeatVar(2), new PowerVar<DrownedPower>(5)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate, CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<DrownedPower>(choiceContext,base.CombatState!.Enemies,base.DynamicVars["DrownedPower"].IntValue,Owner.Creature,this);

        await DamageCmd
            .Attack(DynamicVars.Damage.IntValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(base.CombatState!)
            .WithHitCount(DynamicVars.Repeat.IntValue)
			.Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["DrownedPower"].UpgradeValueBy(1);
        base.DynamicVars.Damage.UpgradeValueBy(1);
    }


}