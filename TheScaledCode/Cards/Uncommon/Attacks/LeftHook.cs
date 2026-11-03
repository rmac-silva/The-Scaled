using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

  
  

public class LeftHook : SetupCard
{
    public LeftHook() : base(0,CardType.Attack,CardRarity.Uncommon,TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new CardsVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>()];
    protected override SetupCardType CardSetupType => SetupCardType.Offensive;

    public override async Task AmbushEffect(AmbushMethodInfo info)
    {
        List<CardModel> generatedCards = [];
        for(int i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            generatedCards.Add(Owner.Creature.CombatState!.CreateCard<LeftHook>(Owner));
        }
        await CardPileCmd.AddGeneratedCardsToCombat(generatedCards,PileType.Hand,Owner);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        AddSetup(cardPlay);
        
    }
    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3);
    }
}