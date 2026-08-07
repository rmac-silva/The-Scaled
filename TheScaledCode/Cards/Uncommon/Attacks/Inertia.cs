using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

public class Inertia : TheScaledCard
{
    public Inertia() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ExertionPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(15m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("Turns",3)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        (await PowerCmd.Apply<InertiaPower>(choiceContext,base.Owner.Creature,DynamicVars["Turns"].BaseValue,base.Owner.Creature,cardPlay.Card))?.IncreaseReductionAmount();

    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Turns"].UpgradeValueBy(1);
    }
}