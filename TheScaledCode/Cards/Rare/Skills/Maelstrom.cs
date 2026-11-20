using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards
{


public class Maelstrom : TheScaledCard
    {
        public Maelstrom() : base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        private readonly List<Type> exceptions = new List<Type>()
        {
            typeof(StunSetup),

        };

        protected override bool HasEnergyCostX => true;


        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new PowerVar<Ambush>(2)
            ];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>()];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            int num = ResolveEnergyXValue();

           //Apply 2X random setups
            var setupPool = ModelDb.AllPowers.Where(p => p is ISetupPower && !exceptions.Contains(p.GetType()));
            

            for (int i = 0; i < DynamicVars["SetupPower"].IntValue * num; i++)
            {
                var randomSetup = Owner.RunState.Rng.GetRng(MegaCrit.Sts2.Core.Entities.Rngs.RunRngType.CombatCardSelection).NextItem(setupPool);
                if(randomSetup == null)
                {
                    continue;
                }
                await PowerCmd.Apply(choiceContext, randomSetup, cardPlay.Target, 1, base.Owner.Creature, this);
            }

            
            await PowerCmd.Apply<TriggerAmbushNextTurn>(choiceContext,cardPlay.Target,1,base.Owner.Creature,this);

        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}
