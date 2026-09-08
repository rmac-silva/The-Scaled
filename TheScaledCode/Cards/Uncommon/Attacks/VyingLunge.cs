using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheScaled.TheScaledCode.Cards;

  
public class VyingLunge : TheScaledCard
{
    public VyingLunge() : base(0,CardType.Attack,CardRarity.Uncommon,TargetType.AnyEnemy)
    {
    }

    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            return !HasBeenPlayedThisTurn;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(base.Owner)];
    private bool HasBeenPlayedThisTurn => CombatManager.Instance.History.CardPlaysFinished.Any((CardPlayFinishedEntry e) => e.CardPlay.Card == this && e.HappenedThisTurn(base.CombatState));


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        if (!HasBeenPlayedThisTurn)
        {
            await PlayerCmd.GainEnergy(1,base.Owner);
        }
    }
    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(4);
    }
}