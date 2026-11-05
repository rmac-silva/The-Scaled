using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;

public class StalkPower : TemporaryDexterityPower, ICustomModel
{
    public override AbstractModel OriginModel => ModelDb.Card<Stalk>();
}