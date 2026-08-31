using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;
public class Ambuscade : SetupCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),new DynamicVar("AmbushEffect",6), new DynamicVar("AmbushAmount",3)];
    
    public Ambuscade() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>(), AmbushHoverTip];

    protected override SetupCardType CardSetupType => SetupCardType.Debuff;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await base.OnPlay(choiceContext, cardPlay);
        
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3);
    }

    protected override Task AmbushEffect(AmbushMethodInfo info)
    {
        if(info.target == null)
        {
            ModLog.Warning(this,"AmbushEffect called with null target.");
            return Task.CompletedTask;
        }

        if(info.applier == null)
        {
            ModLog.Warning(this,"AmbushEffect called with null applier.");
            return Task.CompletedTask;
        }

        PowerCmd.Apply<DrownedPower>(new ThrowingPlayerChoiceContext(), info.target, base.DynamicVars["AmbushEffect"].BaseValue, info.applier, this);
        return Task.CompletedTask;
    }
}