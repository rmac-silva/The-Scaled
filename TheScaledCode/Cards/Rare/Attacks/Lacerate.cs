using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheScaled.TheScaledCode.Cards;

  
public class Lacerate : TheScaledCard
{
    public Lacerate() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override bool ShouldGlowGoldInternal => !HasBeenPlayedThisTurn;

    private bool HasBeenPlayedThisTurn => CombatManager.Instance.History.CardPlaysFinished.Any((CardPlayFinishedEntry e) => e.CardPlay.Card == this && e.HappenedThisTurn(base.CombatState));

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd
            .Attack(base.DynamicVars.Damage.IntValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
			.Execute(choiceContext);
        if(!HasBeenPlayedThisTurn)
        {
            base.BaseReplayCount += IsUpgraded ? 2 : 1;
        }
    }



}