using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheScaled.TheScaledCode.Cards;

public class GutStrike : TheScaledCard
{
    public GutStrike()
        : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromCard<Mud>()];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(32m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new DynamicVar("MudCards", 3),
            new PowerVar<WeakPower>(2),
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");

        //Deal {Damage} damage
        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        //Apply {WeakPower} weak
        await PowerCmd.Apply<WeakPower>(
            choiceContext,
            cardPlay.Target,
            DynamicVars["WeakPower"].BaseValue,
            Owner.Creature,
            this
        );

        //Add {MudCards} Mud Cards to your discard pile
        List<Mud> mudCards = Mud.Create(base.Owner, base.DynamicVars["MudCards"].IntValue, base.CombatState).ToList();
		
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(mudCards,PileType.Discard,base.Owner));
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(10);
    }
}
