using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Extensions;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;



  

public class Ravage : TheScaledCard
{
    public Ravage() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(10),
        new ExtraDamageVar(2),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel c, Creature? _) => CombatManager.Instance.History.Entries.OfType<EnergyGainedEntry>().Where((EnergyGainedEntry e) => e.Actor is not null && e.Actor == c.Owner.Creature).Sum(entry => entry.EnergyGained))
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        foreach(EnergyGainedEntry ent in CombatManager.Instance.History.Entries.OfType<EnergyGainedEntry>())
        {
            ModLog.Info(this,$"Energy gained entry: {ent}");
        }

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

    protected override void OnUpgrade()
    {
        base.DynamicVars.CalculationBase.UpgradeValueBy(4);
    }

    


}