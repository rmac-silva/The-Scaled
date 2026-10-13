using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;
public class Surprise : TheScaledCard
{
    public Surprise() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override bool ShouldGlowGoldInternal => IsPlayable;
    protected override bool IsPlayable => Mud.HasMuddied(this);

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(50,ValueProp.Move)
        ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromAffliction<Muddied>().First()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        CardCmd.ClearAffliction(this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(20);
    }

    


}