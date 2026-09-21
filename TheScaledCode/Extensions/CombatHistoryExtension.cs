using System.Text;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using TheScaled.TheScaledCode.Extensions;

namespace TheScaled.TheScaledCode.Extensions;

public class EnergyGainedEntry : CombatHistoryEntry
{

    public int EnergyGained { get; }
    public override string Description
    {
        get
        {
            StringBuilder stringBuilder = new StringBuilder(Actor?.Player?.Character.Id.Entry + " gained " + EnergyGained);
            return stringBuilder.ToString();
        }
    }

    public EnergyGainedEntry(int energy, int roundNumber, Creature player, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
		: base(player, roundNumber, currentSide, history, players)
	{
		EnergyGained = energy;
	}
}

public static class CombatHistoryExtensions
{
    private static readonly Action<CombatHistory, ICombatState, CombatHistoryEntry> AddDelegate = 
        AccessTools.MethodDelegate<Action<CombatHistory, ICombatState, CombatHistoryEntry>>(
            AccessTools.Method(typeof(CombatHistory), "Add")
        );

    public static void EnergyGained(this CombatHistory history, ICombatState combatState, int amount, Creature player)
    {
        AddDelegate(history, combatState, new EnergyGainedEntry(
            amount, 
            combatState.RoundNumber, 
            player, 
            combatState.CurrentSide, 
            history, 
            combatState.Players
        ));
    }
}