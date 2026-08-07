using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Helpers;

namespace TheScaled.TheScaledCode.Cards;

public class Ambush : TheScaledCard
{
    public Ambush()
        : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            if (base.CombatState == null)
            {
                return false;
            }
            return base.CombatState.HittableEnemies.Any(
                (Creature e) =>
                {
                    if (e.Monster is not null)
                    {
                        return IntentHelper.IntendsToBlock(e.Monster.NextMove);
                    }
                    else
                    {
                        return false;
                    }
                }
            );
        }
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<FrailPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new DynamicVar("GlobalDamage", 9),
            new PowerVar<FrailPower>(1),
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");

        //Deal damage to card target
        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        //Now look at the participants, and fetch those intending to block
        var blockingEnemies = CombatState.Enemies.Where(
            (Creature e) =>
            {
                if (e.Monster is not null)
                {
                    return IntentHelper.IntendsToBlock(e.Monster.NextMove);
                }
                else
                {
                    return false;
                }
            }
        );

        if (blockingEnemies.Count() > 0)
        {
            //Deal global damage to blocking enemies
            await DamageCmd
                .Attack(base.DynamicVars["GlobalDamage"].BaseValue)
                .FromCard(this)
                .TargetingFiltered(blockingEnemies)
                .Execute(choiceContext);

            //Apply 1 frail to the blocking enemies
            await PowerCmd.Apply<FrailPower>(
                choiceContext,
                blockingEnemies,
                DynamicVars["FrailPower"].BaseValue,
                base.Owner.Creature,
                this
            );
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["GlobalDamage"].UpgradeValueBy(2);
    }
}
