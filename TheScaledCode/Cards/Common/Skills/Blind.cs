using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
      
public class Blind : TheScaledCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new PowerVar<VulnerablePower>(1),
                new CardsVar(1)
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<VulnerablePower>(),HoverTipFactory.FromCard<Mud>()];

        public Blind()
            : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target,base.DynamicVars["VulnerablePower"].IntValue,Owner.Creature,this);
        
            await Mud.AddMudCard(PileType.Draw, base.DynamicVars.Cards.IntValue, Owner);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars["VulnerablePower"].UpgradeValueBy(2);
        }
    }
}
