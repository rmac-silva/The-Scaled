using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;



 
public class DwellingPower : TheScaledPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Ambush>()];

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DexterityGain", 0)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if(cardPlay.Card.Owner.Creature == Owner && cardPlay.Card is SetupCard)
        {
            //Gain Dexterity this turn equal to DexterityGain
            await PowerCmd.Apply<TemporaryDexterityPower>(choiceContext,Owner, DynamicVars["DexterityGain"].IntValue, Owner, null);
        }
    }

    public void IncreaseDexterityGain(int amount)
    {
        DynamicVars["DexterityGain"].BaseValue += amount;
    }

    protected override void AfterCloned()
    {
        DynamicVars["DexterityGain"].BaseValue = 0;
        base.AfterCloned();
    }




}
