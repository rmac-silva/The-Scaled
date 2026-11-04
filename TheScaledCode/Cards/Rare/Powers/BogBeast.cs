using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{

public class BogBeast : TheScaledCard
    {
        public BogBeast() : base(2, CardType.Power, CardRarity.Rare, TargetType.None)
        {
        }

        
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromCard<Mud>(true)];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
           //Apply Ancient Form Power
           await PowerCmd.Apply<BogBeastPower>(choiceContext,base.Owner.Creature,1,base.Owner.Creature,this);
           await Mud.AddMudCard(PileType.Hand, DynamicVars.Cards.IntValue, Owner, upgraded: true);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(1);
        }
    }
}
