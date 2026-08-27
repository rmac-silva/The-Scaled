using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class LeftHook : SetupCard
{
    public LeftHook() : base(0,CardType.Attack,CardRarity.Uncommon,TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

    protected override SetupCardType CardSetupType => SetupCardType.Mystery;

    protected override async Task AmbushEffect(AmbushMethodInfo info)
    {
        await CardPileCmd.Add(this,PileType.Hand);
        return;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        
        await base.OnPlay(choiceContext, cardPlay);
    }

    

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3);
    }
}