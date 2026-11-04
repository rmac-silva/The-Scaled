// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MegaCrit.Sts2.Core.Models.Powers;
// using TheScaled.TheScaledCode.Powers;
// using TheScaled.TheScaledCode.Powers.ReusablePowers;

// namespace TheScaled.TheScaledCode.Cards;



//   "THESCALED-STILLNESS.description": "[gold]Setup[/gold]: Apply {DrownedPower} [gold]Drown[/gold]. Whenever this enemy attacks, increase this number by [blue]{DrownIncrease}[/blue].",
//   "THESCALED-STILLNESS.setup": "Apply {DrownedPower} [gold]Drown[/gold]. Whenever this enemy attacks, increase this number by [blue]{DrownIncrease}[/blue].",
//   "THESCALED-STILLNESS.title": "Stillness"
// public class Stillness : SetupCard
// {
//     public Stillness() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
//     {
//     }
//     public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
//     protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrownedPower>(3), new DynamicVar("DrownIncrease",3)];

//     protected override SetupCardType CardSetupType => SetupCardType.Debuff;

//     protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//     {
//         await AddSetup(cardPlay);
//     }

//     public override async Task AmbushEffect(AmbushMethodInfo info)
//     {
//         await PowerCmd.Apply<DrownedPower>(info.choiceContext, info.target, DynamicVars["DrownedPower"].IntValue, Owner.Creature, this);
//     }

//     public override After
// }