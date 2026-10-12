using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards
{
      
public class RaptureOfTheDeep : TheScaledCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new PowerVar<DrownedPower>(5),
                new CardsVar(2)
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<VulnerablePower>(),HoverTipFactory.FromCard<Mud>()];

        public RaptureOfTheDeep()
            : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target,base.DynamicVars["VulnerablePower"].IntValue,Owner.Creature,this);
        
            await Mud.AddMudCard(PileType.Draw, base.DynamicVars.Cards.IntValue, Owner);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Cards.UpgradeValueBy(1);
        }
    }
}
