using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Afflictions;
using TheScaled.TheScaledCode.Extensions;
using TheScaled.TheScaledCode.Powers;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;



  

  
  
public class FromTheDepths : TheScaledCard
{
    public FromTheDepths() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
    }

    

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6m, ValueProp.Move),
		new PowerVar<DrownedPower>(5)
        ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate,CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        ArgumentNullException.ThrowIfNull(this.CombatState, "base.CombatState");

        //Deal damage to all enemies
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
        .FromCard(this, cardPlay)
        .TargetingAllOpponents(this.CombatState)
        .Execute(choiceContext);

        //Apply
        await PowerCmd.Apply<DrownedPower>(choiceContext,this.CombatState.Enemies,base.DynamicVars["DrownedPower"].IntValue,base.Owner.Creature,this);

        

    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3);
        base.DynamicVars["DrownedPower"].UpgradeValueBy(2);
    }

    private static IEnumerable<CardModel> GetMuddiedCards(Player owner)
	{
        ArgumentNullException.ThrowIfNull(owner.PlayerCombatState);
		return owner.PlayerCombatState.AllCards.Where((CardModel c) => c.Type == CardType.Status && c.Pile?.Type != PileType.Exhaust && c.Affliction is Muddied);
	}


}