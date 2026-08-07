using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{

public class Impede : TheScaledCard
    {
        public Impede() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
        {}

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new DynamicVar("StrengthLoss",4),
        ];  
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];


        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

            
            await PowerCmd.Apply<ImpedePower>(choiceContext, cardPlay.Target, base.DynamicVars["StrengthLoss"].BaseValue, base.Owner.Creature, this);
            
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars["StrengthLoss"].UpgradeValueBy(3);
        }
    }
}