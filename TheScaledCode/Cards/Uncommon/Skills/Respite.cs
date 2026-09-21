using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{
    public class Respite : SetupCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new BlockVar(11, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
                new CardsVar(1),
                new DynamicVar("AmbushEffect", 2),
            ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<Ambush>()];

        protected override SetupCardType CardSetupType => SetupCardType.Buff;

        public Respite()
            : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
            await PowerCmd.Apply<DrawCardsNextTurnPower>(
                choiceContext,
                base.Owner.Creature,
                base.DynamicVars.Cards.IntValue,
                base.Owner.Creature,
                this
            );
            await base.AddSetup(choiceContext, cardPlay);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(4);
        }

        protected override async Task AmbushEffect(AmbushMethodInfo info, Dictionary<string,decimal> _)
        {

            if(info.choiceContext == null)
            {
                ModLog.Warning(this,"ChoiceContext provided was null, which should not happen!");
                return;
            }

            await CardPileCmd.Draw(info.choiceContext,base.DynamicVars["AmbushEffect"].IntValue,base.Owner);
        }
    }
}
