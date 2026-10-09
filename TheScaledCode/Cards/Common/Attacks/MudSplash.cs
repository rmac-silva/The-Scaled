using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Afflictions;

namespace TheScaled.TheScaledCode.Cards;

  
public class MudSplash : TheScaledCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(9m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new CardsVar(1),
        ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromAffliction<Muddied>().First()];

    public MudSplash()
        : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");
        
        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        var cards = await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, 0, base.DynamicVars.Cards.IntValue), Muddied.CanAfflictMuddied,this);

        await Mud.MuddyCards(cards.ToList());
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}
