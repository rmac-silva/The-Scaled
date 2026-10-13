using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;
  
public class MakeWaves : TheScaledCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new PowerVar<DrownedPower>(3)];
    
    public MakeWaves() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState);
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await PowerCmd.Apply<DrownedPower>(choiceContext, Owner.Creature,DynamicVars["DrownedPower"].IntValue,Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}