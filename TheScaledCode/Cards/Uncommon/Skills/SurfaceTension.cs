using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards
{





    public class SurfaceTension : TheScaledCard
    {
        public SurfaceTension() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new BlockVar(15,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
                new PowerVar<DrownedPower>(6)
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<DrownedPower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await PowerCmd.Apply<DrownedPower>(choiceContext, Owner.Creature, DynamicVars["DrownedPower"].IntValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(5);
            DynamicVars["DrownedPower"].UpgradeValueBy(1);
        }
    }
}
