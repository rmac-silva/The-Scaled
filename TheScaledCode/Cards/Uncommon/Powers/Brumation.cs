using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
public class Brumation : TheScaledCard
{
    public Brumation() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PlatingPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => 
	[	new PowerVar<PlatingPower>(5)


    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.Owner.Creature.CombatState);
        var enemies = base.Owner.Creature.CombatState.Enemies;
        await PowerCmd.Apply<Ambush>(choiceContext,enemies,-base.DynamicVars["Ambush"].IntValue,base.Owner.Creature, this);

        await PowerCmd.Apply<PlatingPower>(choiceContext, base.Owner.Creature,base.DynamicVars["PlatingPower"].IntValue,base.Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["PlatingPower"].UpgradeValueBy(3);
    }
}