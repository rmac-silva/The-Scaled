using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
      
  
public class Entangle : TheScaledCard
    {
        public override bool GainsBlock => true;
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("DrownAmount",4)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrownedPower>()];

        public Entangle() : base(2, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await CreatureCmd.GainBlock(base.Owner.Creature, DynamicVars.Block, cardPlay);
            await PowerCmd.Apply<DrownedPower>(choiceContext,cardPlay.Target,DynamicVars["DrownAmount"].BaseValue,base.Owner.Creature,cardPlay.Card);
            
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(3);
            base.DynamicVars["DrownAmount"].UpgradeValueBy(2);
        }
    }
}