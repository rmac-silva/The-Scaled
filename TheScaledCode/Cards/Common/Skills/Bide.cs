using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
      
  
public class Bide : TheScaledCard
    {
        public override bool GainsBlock => true;

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                if (base.CombatState == null)
                {
                    return false;
                }
                return base.CombatState.HittableEnemies.Any((Creature e) => e.Monster?.IntendsToAttack ?? false);
            }
        }

    
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("ExertionLossNextTurn",1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ExertionPower>()];

        public Bide() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            ArgumentNullException.ThrowIfNull(cardPlay.Target.Monster, "cardPlay.Target.Monster");
            await CreatureCmd.GainBlock(base.Owner.Creature, DynamicVars.Block, cardPlay);

            //If the enemy intends to attack, give -1 Exertion next turn.
            if(cardPlay.Target.Monster.IntendsToAttack)
            {
                await PowerCmd.Apply<BidePower>(choiceContext, base.Owner.Creature, DynamicVars["ExertionLossNextTurn"].BaseValue, base.Owner.Creature, cardPlay.Card);
            }
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(3);
        }
    }
}