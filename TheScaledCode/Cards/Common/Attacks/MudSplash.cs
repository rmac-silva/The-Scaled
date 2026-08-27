using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Cards;

  
public class MudSplash : TheScaledCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(7m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new DynamicVar("MudAmount", 2m),
        ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Mud>()];

    public MudSplash()
        : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");
        
        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this,cardPlay)
            .TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        List<CardModel> list = new List<CardModel>();
        for (int i = 0; i < base.DynamicVars["MudAmount"].IntValue; i++)
        {
            CardModel card = base.CombatState.CreateCard<Mud>(base.Owner);
            list.Add(card);
        }

        await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, base.Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3);
    }
}
