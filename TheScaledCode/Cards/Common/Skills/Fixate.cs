using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Enchantments;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

public class Fixate : SetupCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(1), new DynamicVar("MudAmount", 1),new DamageVar(2,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(),HoverTipFactory.FromCard<Mud>()];

    protected override SetupCardType CardSetupType => SetupCardType.Offensive;

    public Fixate() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");

        //Appyl 1 weak
        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, base.DynamicVars.Weak.IntValue,base.Owner.Creature,this);

        //Create 1 mud card
        await Mud.AddMudCard(PileType.Hand, base.DynamicVars["MudAmount"].IntValue, base.Owner);

        //Call setup
        await base.OnPlay(choiceContext, cardPlay);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(1);
    }

    protected override async Task AmbushEffect(AmbushMethodInfo info)
    {
        if(info.applier is null )
        {
            ModLog.Warning(this,$"Warning. Applier is null when calling AmbushEffect.");
            return;
        }

        if(info.applier.Player is null)
        {
            ModLog.Warning(this,$"Warning. Applier.Player is null when calling AmbushEffect.");
            return;
        }

        if(info.target is null)
        {
            ModLog.Warning(this,$"Warning. Target is null when calling AmbushEffect.");
            return;
        }

        var muddiedCards = PileType.Deck.GetPile(info.applier.Player).Cards.Where(c => c.Enchantment != null && c.Enchantment is Muddied).ToList();
        var damage = base.DynamicVars.Damage.BaseValue * muddiedCards.Count;

        await DamageCmd.Attack(damage)
            .FromCard(this,null)
            .Targeting(info.target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(info.choiceContext);

        return;



    }
}