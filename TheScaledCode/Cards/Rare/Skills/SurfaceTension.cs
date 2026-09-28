using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers.Cards;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards
{

  
  
  
  
public class SurfaceTension : TheScaledCard
    {
        public SurfaceTension() : base(2, CardType.Skill, CardRarity.Rare, TargetType.None)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new BlockVar(18,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            ];
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<DrownedPower>()];
        


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
           //Gain 18(25) Block
            await CreatureCmd.GainBlock(base.Owner.Creature,base.DynamicVars.Block,cardPlay);
           //Apply Surface Tension Power
           await PowerCmd.Apply<SurfaceTensionPower>(choiceContext,base.Owner.Creature,1,base.Owner.Creature,this);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(7);
        }
    }
}
