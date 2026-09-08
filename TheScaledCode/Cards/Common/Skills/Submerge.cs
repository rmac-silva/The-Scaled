using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class Submerge : TheScaledCard
{
    public Submerge() : base(1, CardType.Skill, CardRarity.Common, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(1), new PowerVar<DrownedPower>(2), new RepeatVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<DrownedPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState,"base.CombatState");
        
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        for (int i = 0; i < base.DynamicVars.Repeat.IntValue; i++)
		{
			Creature? enemy = base.Owner.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies);
			if (enemy == null)
			{
				continue;
			}
			
			await PowerCmd.Apply<WeakPower>(choiceContext, enemy, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
			await PowerCmd.Apply<DrownedPower>(choiceContext, enemy, base.DynamicVars["DrownedPower"].IntValue, base.Owner.Creature, this);
            await Cmd.Wait(0.5f);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Repeat.UpgradeValueBy(1);
    }
}