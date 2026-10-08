using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using TheScaled.TheScaledCode.Powers;
using TheScaled.TheScaledCode.Powers.Cards;

namespace TheScaled.TheScaledCode.Cards
{


public class Maelstrom : TheScaledCard
    {
        public Maelstrom() : base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        protected override bool HasEnergyCostX => true;


        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [
                new PowerVar<SetupPower>(2)
            ];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Ambush>()];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            int num = ResolveEnergyXValue();

           //Apply 2X random setups
            
            var setupPool = base.Owner.Character.CardPool
                    .GetUnlockedCards(
                        base.Owner.UnlockState,
                        base.Owner.RunState.CardMultiplayerConstraint)
                    .OfType<SetupCard>().Where( (SetupCard c) => c.CanBeGeneratedInCombat);

            var cardModels = CardFactory.GetForCombat(
                base.Owner,
                setupPool,
                base.DynamicVars["SetupPower"].IntValue * num,
                base.Owner.RunState.Rng.CombatCardGeneration);
        

            foreach(CardModel c in cardModels)
            {

                if (c is not SetupCard setupCard)
                {
                    continue;
                }

                var ambPwr = SetupCard.GetAmbushPowerForApplier(base.Owner.Creature,cardPlay.Target);

                if(ambPwr is null)
                {
                    ambPwr = await PowerCmd.Apply<Ambush>(choiceContext,cardPlay.Target,1,base.Owner.Creature,this);
                    if(ambPwr != null)
                    {
                        ambPwr.SetAmount(0);
                    }
                }

                if(ambPwr != null && c is SetupCard)
                {
                    var ambEntry = new AmbushEntry(setupCard.AmbushEffect,setupCard,setupCard.SetupData);
                    await ambPwr.AddAmbushEffect(ambEntry,setupCard.GetHovertip(cardPlay.Target));
                }
            }


            await PowerCmd.Apply<TriggerAmbushNextTurn>(choiceContext,cardPlay.Target,1,base.Owner.Creature,this);

        }

        protected override void OnUpgrade()
        {
            base.AddKeyword(CardKeyword.Retain);
        }
    }
}
