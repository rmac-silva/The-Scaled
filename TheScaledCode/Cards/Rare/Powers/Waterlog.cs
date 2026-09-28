using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers.Cards;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards
{

  
  
  
  
  
  
public class Waterlog : TheScaledCard
    {
        public Waterlog() : base(2, CardType.Power, CardRarity.Rare, TargetType.None)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SlipperyPower>(2)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<SlipperyPower>()];
        


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
           //Apply Ancient Form Power
           await PowerCmd.Apply<SlipperyPower>(choiceContext,base.Owner.Creature,base.DynamicVars["SlipperyPower"].IntValue,base.Owner.Creature,this);
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
