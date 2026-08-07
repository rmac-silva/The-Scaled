using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;

/// <summary>
///
/// Note: This needs to have a minimum value of 1, otherwise you will infinitely add strength
/// to your character with any attack as it is always over the threshold.
/// </summary>

public class ImpedePower : TemporaryStrengthPower, ICustomModel
{
	public override AbstractModel OriginModel => ModelDb.Card<Impede>();

	protected override bool IsPositive => false;
}