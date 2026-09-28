using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers.Cards;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards
{

  
  
  
  
  
public class AncientForm : TheScaledCard
    {
        public AncientForm() : base(3, CardType.Power, CardRarity.Rare, TargetType.None)
        {
        }

        
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<AncientFormPower>()];
        


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
           //Apply Ancient Form Power
           await PowerCmd.Apply<AncientFormPower>(choiceContext,base.Owner.Creature,1,base.Owner.Creature,this);
        }

        protected override void OnUpgrade()
        {
            base.Keywords.AddItem(CardKeyword.Innate);
        }
    }
}
