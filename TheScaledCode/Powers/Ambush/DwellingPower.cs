using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheScaled.TheScaledCode.Powers;



 
public class DwellingPower : TheScaledPower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Ambush>()];

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DexterityGain", 0)];

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if(applier == Owner && power is ISetupPower && amount > 0)
        {
            await PowerCmd.Apply<DwellingDexterityPower>(choiceContext,Owner, DynamicVars["DexterityGain"].IntValue, Owner, null);
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
