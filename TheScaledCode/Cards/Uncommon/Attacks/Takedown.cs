using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;

  
public class Takedown : TheScaledCard
{
    public Takedown() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            if (base.CombatState == null)
            {
                return false;
            }
            return base.CombatState.HittableEnemies.Any((Creature e) => e.HasPower<DrownedPower>());
        }
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        var nrHits = cardPlay.Target.HasPower<DrownedPower>() ? 2 : 1;

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .WithHitCount(nrHits)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2);
    }

}