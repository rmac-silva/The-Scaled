using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;


public class Chomp : SetupCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [new DamageVar(11m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new PowerVar<VulnerablePower>(2), new PowerVar<WeakPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [AmbushHoverTip, HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<WeakPower>()];

    protected override SetupCardType CardSetupType { get => SetupCardType.Debuff; }

    public Chomp()
        : base(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await base.AddSetup(choiceContext, cardPlay);
        

    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }


    protected override async Task AmbushEffect(AmbushMethodInfo info, Dictionary<string,int> _)
    {
        if (info.target == null)
        {
            ModLog.Warning(this, "AmbushEffect called with null target.");
            return;
        }

        await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), info.target, base.DynamicVars.Vulnerable.IntValue, base.Owner.Creature, null);
        await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), info.target, base.DynamicVars.Weak.IntValue, base.Owner.Creature, null);
        return;
    }
}
