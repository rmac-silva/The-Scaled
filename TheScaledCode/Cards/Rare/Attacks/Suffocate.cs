using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;



  
public class Suffocate : TheScaledCard
{
    public Suffocate() : base(9, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(57),
        new ExtraDamageVar(7),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => card.CombatState?.Enemies.Where((Creature c) => c.IsAlive).Sum((Creature c) => c.GetPower<Ambush>()?.NumSetupsForCreature) ?? 0),
        new EnergyVar(1)
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        ArgumentNullException.ThrowIfNull(this.CombatState, "this.CombatState");
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd
            .Attack(base.DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        var a = cardPlay.Target.GetPower<Ambush>();

        if(a is not null)
        {
            await a.TriggerAmbushExternal();
        }

    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if(cardPlay.Card != this)
        {
            ArgumentNullException.ThrowIfNull(base.Owner.Creature.CombatState);
            var totalSetups = base.Owner.Creature.CombatState.Enemies.Sum((Creature c) => c.GetPower<Ambush>()?.NumSetupsForCreature ?? 0);
            SetCost(totalSetups);
        }
            return Task.CompletedTask;
    }

    public override  Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if(card != this)
        {
            return Task.CompletedTask;
        }

        ArgumentNullException.ThrowIfNull(base.Owner.Creature.CombatState);

        var totalSetups = base.Owner.Creature.CombatState.Enemies.Sum((Creature c) => c.GetPower<Ambush>()?.NumSetupsForCreature ?? 0);
        SetCost(totalSetups);
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this)
		{
			return Task.CompletedTask;
		}

        ArgumentNullException.ThrowIfNull(base.Owner.Creature.CombatState);

        var totalSetups = base.Owner.Creature.CombatState.Enemies.Sum((Creature c) => c.GetPower<Ambush>()?.NumSetupsForCreature ?? 0);
        SetCost(totalSetups);
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.CalculationBase.UpgradeValueBy(12);
    }

    private void SetCost(int setups)
    {
        base.EnergyCost.SetThisTurnOrUntilPlayed(9-setups);
    }


}