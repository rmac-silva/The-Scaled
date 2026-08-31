using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class Concussion : SetupCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<FrailPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new PowerVar<VulnerablePower>(1), new PowerVar<FrailPower>(1)];

    protected override SetupCardType CardSetupType => SetupCardType.Debuff;

    public Concussion() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await base.OnPlay(choiceContext, cardPlay);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Vulnerable.UpgradeValueBy(1);
        base.DynamicVars["FrailPower"].UpgradeValueBy(1);
    }

    protected override async Task AmbushEffect(AmbushMethodInfo info)
    {
        if(info.target is null)
        {
            ModLog.Warning(this,"AmbushEffect called with null target");
            return;
        }

        if(info.applier is null)
        {
            ModLog.Warning(this,"AmbushEffect called with null applier");
            return;
        }

        await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), info.target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), info.target, base.DynamicVars["FrailPower"].BaseValue, base.Owner.Creature, this);

        return;
    }
}