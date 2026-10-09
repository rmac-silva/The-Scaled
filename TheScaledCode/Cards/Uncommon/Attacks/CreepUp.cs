using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;
//Deal {Damage:diff()} damage. The next [gold]Afflicted[/gold] card you play costs [blue]0[/blue] [gold]Energy[/gold].
public class CreepUp : TheScaledCard
{
    public CreepUp() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(18,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new EnergyVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if(card != this)
        {
            return Task.CompletedTask;
        }

        if(base.IsClone)
        {
            return Task.CompletedTask;
        }

        int amount = CardPile.Get(PileType.Hand,Owner)?.Cards.Count(c => c.Affliction != null && c.Affliction is Muddied) ?? 0;

        SetCostAt(base.EnergyCost.Canonical - amount);

        return Task.CompletedTask;
    }

    public override Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if(card.Owner != Owner)
        {
            return Task.CompletedTask;
        }

        if(card.Pile is null)
        {
            return Task.CompletedTask;
        }

        if(card.Pile.Type == PileType.Hand && card.Affliction != null && card.Affliction is Muddied)
        {
            int amount = CardPile.Get(PileType.Hand,Owner)?.Cards.Count(c => c.Affliction != null && c.Affliction is Muddied) ?? 0;

            SetCostAt(base.EnergyCost.Canonical  - amount);
        }

        return Task.CompletedTask;
    }

    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if(card.Owner != Owner)
        {
            return Task.CompletedTask;
        }

        int amount = CardPile.Get(PileType.Hand,Owner)?.Cards.Count(c => c.Affliction != null && c.Affliction is Muddied) ?? 0;

        SetCostAt(base.EnergyCost.Canonical  - amount);
        
        return Task.CompletedTask;

    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if(cardPlay.Card.Owner != Owner)
        {
            return Task.CompletedTask;
        }

        int amount = CardPile.Get(PileType.Hand,Owner)?.Cards.Count(c => c.Affliction != null && c.Affliction is Muddied) ?? 0;

        SetCostAt(base.EnergyCost.Canonical - amount);

        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(6);
    }

    private void SetCostAt(int amount)
	{
		base.EnergyCost.SetThisTurn(amount);
	}


}