using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;



  
public class Dwelling : TheScaledCard
{
    public Dwelling() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DexterityGain", 2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        (await PowerCmd.Apply<DwellingPower>(choiceContext, base.Owner.Creature, 1, base.Owner.Creature, this))?.IncreaseDexterityGain(DynamicVars["DexterityGain"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DexterityGain"].UpgradeValueBy(1);
    }

    
}