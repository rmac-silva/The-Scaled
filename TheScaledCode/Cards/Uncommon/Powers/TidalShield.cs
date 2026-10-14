using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class TidalShield : TheScaledCard
{
    public TidalShield() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BlockGain",1)];


    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<TidalShieldPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<TidalShieldPower>(choiceContext, Owner.Creature, base.DynamicVars["BlockGain"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BlockGain"].UpgradeValueBy(1);
    }
}