using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;

  
public class Takedown : TheScaledCard
{
    public Takedown() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new PowerVar<DrownedPower>(5)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");


        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        NDrownImpactVfx? child = NDrownImpactVfx.Create(cardPlay.Target);
        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
        await PowerCmd.Apply<DrownedPower>(choiceContext, cardPlay.Target, base.DynamicVars["DrownedPower"].BaseValue, Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2);
        base.DynamicVars["DrownedPower"].UpgradeValueBy(1);
    }

}