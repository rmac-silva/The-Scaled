using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Afflictions;
using TheScaled.TheScaledCode.Powers;


namespace TheScaled.TheScaledCode.Cards;


  
public class NaturalHabitat : TheScaledCard
{
    public NaturalHabitat() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<NaturalHabitatPower>(),
        HoverTipFactory.FromAffliction<Muddied>().First()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<NaturalHabitatPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.AddKeyword(CardKeyword.Innate);
    }
}