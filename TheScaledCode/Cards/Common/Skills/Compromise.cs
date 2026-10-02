using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

public class Compromise : TheScaledCard
{

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(base.Owner)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new EnergyVar(2)];

    protected override bool ShouldGlowGoldInternal => WillTrigger;

    private bool WillTrigger
	{
		get
		{
			int num = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Player == base.Owner);
			return num < 1;
		}
	}
    public Compromise() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        if(WillTrigger)
        {
            await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext,base.Owner.Creature,base.DynamicVars.Energy.IntValue,base.Owner.Creature,this);
            await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext,base.Owner.Creature,base.DynamicVars.Cards.IntValue,base.Owner.Creature,this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Energy.UpgradeValueBy(1);
    }
}