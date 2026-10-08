using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Relics;

  
public class PredatoryInstinct : TheScaledRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override RelicModel GetUpgradeReplacement() => ModelDb.Relic<ApexInstinct>();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("EnemyAmbushGain", 2m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>(base.DynamicVars["EnemyAmbushGain"].IntValue)];

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        ArgumentNullException.ThrowIfNull(base.Owner.PlayerCombatState);

        if (!participants.Contains(base.Owner.Creature) || base.Owner.PlayerCombatState.TurnNumber > 1)
		{
			return;
		}

        ArgumentNullException.ThrowIfNull(base.Owner.Creature.CombatState);
        foreach (Creature hittableEnemy2 in base.Owner.Creature.CombatState.HittableEnemies)
		{
			await PowerCmd.Apply<Ambush>(choiceContext, hittableEnemy2, base.DynamicVars["EnemyAmbushGain"].IntValue, base.Owner.Creature, null);
            Flash();
		}
    }
}