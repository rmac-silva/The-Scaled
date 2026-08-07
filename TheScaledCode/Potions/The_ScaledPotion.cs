using BaseLib.Abstracts;
using BaseLib.Utils;
using TheScaled.TheScaledCode.Character;

namespace TheScaled.TheScaledCode.Potions;

/// <summary>
/// This represents an abstract potion for The Scaled character.
/// </summary>
[Pool(typeof(TheScaledPotionPool))]
public abstract class TheScaledPotion : CustomPotionModel;