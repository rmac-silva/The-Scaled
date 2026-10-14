using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Powers.ReusablePowers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class AbyssalPull : TheScaledCard
{
    public AbyssalPull() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new PowerVar<DrownedPower>(15)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate, CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>(),HoverTipFactory.Static(StaticHoverTip.Fatal)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        bool shouldTriggerFatal = cardPlay.Target!.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
        
        await PowerCmd.Apply<DrownedPower>(choiceContext,cardPlay.Target!,base.DynamicVars["DrownedPower"].IntValue,Owner.Creature,this);

        AttackCommand attackCommand = await DamageCmd
            .Attack(DynamicVars.Damage.IntValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
			.Execute(choiceContext);

        if (shouldTriggerFatal && attackCommand.Results.SelectMany( (List<DamageResult> r) => r).Any((DamageResult r) => r.WasTargetKilled))
		{
			await PowerCmd.Apply<DrownedPower>(choiceContext,CombatState!.Enemies,base.DynamicVars["DrownedPower"].IntValue,Owner.Creature,this);
		}
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(10);
    }


}