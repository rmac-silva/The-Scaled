using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Ancients;

//(0E Ancient Attack): Deal 3 damage twice. At the start of your next turn, increase this card’s damage by 2(3) and return it to your hand.

  
  

public class MeteorHammer : AncientCard
{
    public MeteorHammer() : base(0,CardType.Attack,CardRarity.Ancient,TargetType.AnyEnemy)
    {
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("DamageIncrease",2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

		await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .Targeting(cardPlay.Target)
			.WithHitCount(2)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);

		
	}

	public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
	{
		if (player == base.Owner && CombatManager.Instance.History.CardPlaysFinished.Any((CardPlayFinishedEntry e) => e.HappenedLastPlayerTurn(base.Owner) && e.CardPlay.Card == this))
		{
			CardPile? pile = base.Pile;
			if (pile == null || pile.Type != PileType.Hand)
			{
				base.DynamicVars.Damage.BaseValue += base.DynamicVars["DamageIncrease"].BaseValue;
				await CardPileCmd.Add(this, PileType.Hand);
			}
		}
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars["DamageIncrease"].UpgradeValueBy(1m);
	}
}