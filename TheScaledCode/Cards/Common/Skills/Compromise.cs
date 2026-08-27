using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

public class Compromise : TheScaledCard
{

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(base.Owner)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Ambush>(1), new EnergyVar(2)];
    public Compromise() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        //Reduce Ambush on all enemies by 1
        ArgumentNullException.ThrowIfNull(base.Owner.Creature.CombatState);
        var enemies = base.Owner.Creature.CombatState.Enemies;
        await PowerCmd.Apply<Ambush>(choiceContext,enemies,-base.DynamicVars["Ambush"].IntValue,base.Owner.Creature, this);

        //Gain energy next turn
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Energy.UpgradeValueBy(1);
    }
}