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

  
  
  
  
public class DeathFromBelow : TheScaledCard
{
    public DeathFromBelow() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }


    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new RepeatVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        var atk = await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .TargetingRandomOpponents(base.CombatState!)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        var totalDamage = atk.Results.Sum( r => r.Sum( h => h.UnblockedDamage));
        NDrownImpactVfx? child = NDrownImpactVfx.Create(cardPlay.Target!);
        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
        await PowerCmd.Apply<DrownedPower>(choiceContext,cardPlay.Target,totalDamage,Owner.Creature,this);

    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Repeat.UpgradeValueBy(1);
    }
}