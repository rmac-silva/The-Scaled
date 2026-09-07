using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace TheScaled.TheScaledCode.Relics.Ancient;

[Pool(typeof(EventRelicPool))]
public class RoyalFlush : CustomRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
        ArgumentNullException.ThrowIfNull(base.Owner.PlayerCombatState,"base.Owner.PlayerCombatState");
		if (player == base.Owner && base.Owner.PlayerCombatState.TurnNumber <= 1)
		{
			IEnumerable<CardModel> list = PileType.Hand.GetPile(base.Owner).Cards;
			if (list.Count() != 0)
			{
				await CardCmd.DiscardAndDraw(choiceContext, list, list.Count());
			}
		}
	}

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (participants.Contains(base.Owner.Creature))
		{
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);	
		}
	}
}