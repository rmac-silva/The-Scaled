using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheScaled.TheScaledCode.Cards
{
    public class FeintStrike : TheScaledCard
    {
        public FeintStrike()
            : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new DamageVar(8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
                new BlockVar(8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
                new CardsVar(1)
            ];
        
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> {CardTag.Strike};

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await DamageCmd
                .Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this,cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await CreatureCmd.GainBlock(base.Owner.Creature,base.DynamicVars.Block,cardPlay);

            await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext,base.Owner.Creature,base.DynamicVars.Cards.IntValue,base.Owner.Creature,this);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Damage.UpgradeValueBy(3m);
            base.DynamicVars.Block.UpgradeValueBy(3m);
        }
    }
}
