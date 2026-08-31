using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Cards;
namespace TheScaled.TheScaledCode.Powers;
public class RetaliatePower : TemporaryStrengthPower, ICustomModel
{
	public override AbstractModel OriginModel => ModelDb.Card<Retaliate>();

	protected override bool IsPositive => false;
}