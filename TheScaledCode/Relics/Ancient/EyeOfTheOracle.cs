using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using TheScaled.TheScaledCode.Character;

namespace TheScaled.TheScaledCode.Relics.Ancient;

[Pool(typeof(EventRelicPool))]
public class EyeOfTheOracle : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
}