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

namespace TheScaled.TheScaledCode.Cards;



  

  
public class Lacerate : TheScaledCard
{
    public Lacerate() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    private decimal _extraDamageFromLaceratePlays;

	private decimal ExtraDamageFromLaceratePlays
	{
		get
		{
			return _extraDamageFromLaceratePlays;
		}
		set
		{
			AssertMutable();
			_extraDamageFromLaceratePlays = value;
		}
	}

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(2m, ValueProp.Move),
		new DynamicVar("Increase", 3m),
        new RepeatVar(2)
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        ArgumentNullException.ThrowIfNull(base.Owner.PlayerCombatState, "Owner.PlayerCombatState");

        //Deal damage twice
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
        .FromCard(this, cardPlay)
        .Targeting(cardPlay.Target)
        .WithHitCount(base.DynamicVars.Repeat.IntValue)
        .Execute(choiceContext);

        //Get muddied cards
        var muddiedCards = GetMuddiedCards(base.Owner);

        //Pick a random one
        var randomCard = muddiedCards.ToList().StableShuffle(base.Owner.RunState.Rng.Shuffle)
				.FirstOrDefault();

        //Transform into a Lacerate
        if(randomCard != null)
        {
            await CardCmd.TransformTo<Lacerate>(randomCard);
        }

        //Display said transformed card

        //Increase the damage of ALL Lacerate cards by 3(5) for the rest of combat. 
		IEnumerable<Lacerate> enumerable = base.Owner.PlayerCombatState.AllCards.OfType<Lacerate>();
		decimal baseValue = base.DynamicVars["Increase"].BaseValue;
		foreach (Lacerate item in enumerable)
		{
			item.BuffFromClawPlay(baseValue);
		}

    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Increase"].UpgradeValueBy(2);
    }

    protected override void AfterDowngraded()
	{
		base.AfterDowngraded();
		base.DynamicVars.Damage.BaseValue += ExtraDamageFromLaceratePlays;
	}

    private void BuffFromClawPlay(decimal extraDamage)
	{
		base.DynamicVars.Damage.BaseValue += extraDamage;
		ExtraDamageFromLaceratePlays += extraDamage;
	}

    private static IEnumerable<CardModel> GetMuddiedCards(Player owner)
	{
        ArgumentNullException.ThrowIfNull(owner.PlayerCombatState);
		return owner.PlayerCombatState.AllCards.Where((CardModel c) => c.Pile?.Type != PileType.Exhaust && c.Affliction is Muddied && c is not Lacerate);
	}


}