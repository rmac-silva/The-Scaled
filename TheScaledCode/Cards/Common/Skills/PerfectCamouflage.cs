using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheScaled.TheScaledCode.Cards
{
      
  
  
public class PerfectCamouflage : TheScaledCard
    {
        public override bool GainsBlock => true;
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(11m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("StrengthLoss",2)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

        public PerfectCamouflage() : base(0, CardType.Skill, CardRarity.Common, TargetType.None)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(base.Owner.Creature, DynamicVars.Block, cardPlay);
            //Player loses Strength
            await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, -DynamicVars["StrengthLoss"].BaseValue, base.Owner.Creature, cardPlay.Card);
            
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(3);
        }
    }
}