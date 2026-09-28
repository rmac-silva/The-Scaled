using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Afflictions;
using TheScaled.TheScaledCode.Powers;
using TheScaled.TheScaledCode.Powers.Cards;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

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
                new PowerVar<SetupPower>(2),
                new PowerVar<Ambush>(3)
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
                    .OfType<SetupCard>();

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

                ModLog.Info(this,$"Selected {c.Title} for random setup!");

                var ambPwr = cardPlay.Target.GetPower<Ambush>();

                if(ambPwr != null && c is SetupCard)
                {
                    var ambEntry = new AmbushEntry(setupCard.AmbushEffect,setupCard,setupCard.SetupData);
                    await ambPwr.AddAmbushEffect(ambEntry,setupCard.GetHovertip(cardPlay.Target));
                    CardCmd.Preview(c,0.4f);
                    await Cmd.Wait(0.4f);
                }
            }

            await PowerCmd.Apply<AmbushNextTurn>(choiceContext,cardPlay.Target,base.DynamicVars["Ambush"].IntValue * num,base.Owner.Creature,this);


            
        }

        protected override void OnUpgrade()
        {
            base.Keywords.AddItem(CardKeyword.Retain);
        }
    }
}
