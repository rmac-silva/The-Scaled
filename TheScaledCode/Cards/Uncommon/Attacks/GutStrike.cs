using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Afflictions;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class GutStrike : TheScaledCard
{
    public GutStrike() : base(2,CardType.Attack,CardRarity.Uncommon,TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>(),HoverTipFactory.FromAffliction<Muddied>().First()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(24m,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),new PowerVar<Ambush>(5),new DynamicVar("MudAmount",3)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        
        await Mud.AddMudCard(PileType.Discard,base.DynamicVars["MudAmount"].IntValue,base.Owner);
    }

    

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(12);
    }
}