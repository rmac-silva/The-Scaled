using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;

public class DwellingDexterityPower : TemporaryDexterityPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Dwelling>();

}
