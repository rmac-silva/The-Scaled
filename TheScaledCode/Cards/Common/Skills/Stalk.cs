using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
      
  
  
  
public class Stalk : SetupCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(5,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),new PowerVar<FrailPower>(2)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FrailPower>()];

        protected override SetupCardType CardSetupType => SetupCardType.Debuff;

        public Stalk() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);

            await base.OnPlay(choiceContext, cardPlay);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(3);
            base.DynamicVars["FrailPower"].UpgradeValueBy(1);
        }

        protected override async Task AmbushEffect(AmbushMethodInfo info)
        {
            if(info.target is null)
            {
                ModLog.Warning(this,$"Warning. info.target is null when calling AmbushEffect.");
                return;
            }

            if(info.applier is null )
            {
                ModLog.Warning(this,$"Warning. info.applier is null when calling AmbushEffect.");
                return;
            }

            await PowerCmd.Apply<FrailPower>(info.choiceContext is null ? new ThrowingPlayerChoiceContext() : info.choiceContext, info.target, base.DynamicVars["FrailPower"].IntValue, base.Owner.Creature, this);
        }
    }
}