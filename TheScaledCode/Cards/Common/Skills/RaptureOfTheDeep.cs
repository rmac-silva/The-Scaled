using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards
{
      
  
public class RaptureOfTheDeep : TheScaledCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new PowerVar<DrownedPower>(4),
                new CardsVar(2)
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<DrownedPower>()];

        public RaptureOfTheDeep()
            : base(0, CardType.Skill, CardRarity.Common, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            NDrownImpactVfx? child = NDrownImpactVfx.Create(Owner.Creature);
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
            await PowerCmd.Apply<DrownedPower>(choiceContext, Owner.Creature, base.DynamicVars["DrownedPower"].BaseValue, Owner.Creature, this);
            await CardPileCmd.Draw(choiceContext,DynamicVars.Cards.IntValue,Owner);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Cards.UpgradeValueBy(1);
        }
    }
}
