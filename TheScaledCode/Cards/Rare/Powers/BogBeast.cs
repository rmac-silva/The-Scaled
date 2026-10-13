using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{

public class BogBeast : TheScaledCard
    {
        public BogBeast() : base(3, CardType.Power, CardRarity.Rare, TargetType.None)
        {
        }

        
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<BogBeastPower>(), HoverTipFactory.FromCard<Mud>(true)];
        


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
           //Apply Ancient Form Power
           await PowerCmd.Apply<BogBeastPower>(choiceContext,base.Owner.Creature,1,base.Owner.Creature,this);
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
