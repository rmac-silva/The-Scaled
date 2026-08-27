using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;

public class RuggedScalesPower : TemporaryThornsPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<RuggedScales>();
}