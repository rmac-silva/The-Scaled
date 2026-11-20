using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class FollowUpStrike : SetupCard
{
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> {CardTag.Strike};
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("AmbushEffect",7)];


    public FollowUpStrike() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        
        await PowerCmd.Apply<DamageSetup>(choiceContext, cardPlay.Target, base.DynamicVars["AmbushEffect"].IntValue, Owner.Creature, this);
        
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["AmbushEffect"].UpgradeValueBy(3);
    }

}