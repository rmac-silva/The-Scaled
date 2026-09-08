using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{

  
public class Vigil : TheScaledCard
    {
        public Vigil() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.None)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new BlockVar(9, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
                new PowerVar<BlurPower>(2),
            ];
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<BlurPower>()];



        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            ArgumentNullException.ThrowIfNull(
                base.Owner.Creature.CombatState,
                "Owner.Creature.CombatState"
            );

            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);

            await PowerCmd.Apply<BlurPower>(choiceContext,cardPlay.Target,base.DynamicVars["BlurPower"].IntValue,base.Owner.Creature,this);
            
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(3);
        }
    }
}
