using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using TheScaled.TheScaledCode.Character;
using TheScaled.TheScaledCode.Extensions;

namespace TheScaled.TheScaledCode.Potions;

/// <summary>
/// This represents an abstract potion for The Scaled character.
/// </summary>
[Pool(typeof(TheScaledPotionPool))]
public abstract class TheScaledPotion : CustomPotionModel
{
    public override string CustomPackedImagePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
    public override string CustomLargeImagePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
    public override string? CustomPackedOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".PotionImagePath();
}