using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class RavenousThrash : TheScaledCard
{
    public RavenousThrash() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }
    protected override bool HasEnergyCostX => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(1,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),new PowerVar<DrownedPower>(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int num = IsUpgraded ? ResolveEnergyXValue() + 1 : ResolveEnergyXValue();
        NDrownImpactVfx? child = NDrownImpactVfx.Create(cardPlay.Target!);
        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
        await PowerCmd.Apply<DrownedPower>(choiceContext,cardPlay.Target!,base.DynamicVars["DrownedPower"].IntValue * num,Owner.Creature,this);

        await DamageCmd
            .Attack(DynamicVars.Damage.IntValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
            .WithHitCount(num)
			.Execute(choiceContext);
    }


}