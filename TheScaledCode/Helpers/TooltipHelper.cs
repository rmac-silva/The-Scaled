using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using static TheScaled.TheScaledCode.Cards.SetupCard;
namespace TheScaled.TheScaledCode.Helpers;
public static class TooltipHelper
{
    public static readonly string WeakAttackIcon = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/attack/intent_attack_1.tres");
    public static readonly string NormalAttackIcon = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/attack/intent_attack_2.tres");
    public static readonly string StrongAttackIcon = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/attack/intent_attack_3.tres");
    public static readonly string HugeAttackIcon = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/attack/intent_attack_4.tres");
    public static readonly string CrushingAttackIcon = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/attack/intent_attack_5.tres");
    public static readonly string DefenseIcon = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/intent_defend.tres");
    public static readonly string BuffIcon = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/intent_buff.tres");
    public static readonly string DebuffIcon = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/intent_debuff.tres");
    public static readonly string MysteryIcon = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/intent_unknown.tres");

    /// <summary>
    /// Creates a new HoverTip instance with the specified title, description, debuff and possible Texture2D Icon.
    /// Useful for recreating existing HoverTips with a new title
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="isDebuff"></param>
    /// <param name="icon"></param>
    /// <returns></returns>
    public static HoverTip CreateHoverTooltip(String title, String description, bool isDebuff = false, Texture2D? icon = null)
    {
        var baseTooltip = new HoverTip(
            ModelDb.Power<StrengthPower>(),
            "Some test description",
            false
        );

        var newTooltip = baseTooltip with { Id = "CardTracker_CustomTip", IsSmart = false };

        var traverse = Traverse.Create(newTooltip);

        traverse.Property("Title").SetValue(title);
        traverse.Property("Description").SetValue(description);
        traverse.Property("Icon").SetValue(icon);

        newTooltip = traverse.GetValue<HoverTip>();
        newTooltip.IsDebuff = isDebuff;
        newTooltip.Id = $"CardTracker_CustomTip_{title.GetHashCode()}";
        ModLog.Info(null, $"Recreated new hover tip: {newTooltip}.\nWith Icon: {icon?.ResourcePath ?? "null"}");
        return newTooltip;
    }

    /// <summary>
    /// Creates a new HoverTip instance with the specified title, description, icon type, and debuff status.
    /// Useful for creating HoverTips from scratch.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="iconType"></param>
    /// <param name="isDebuff"></param>
    /// <returns></returns>
    public static HoverTip CreateHoverTooltip(String title, String description, SetupCardType iconType, bool isDebuff = false)
    {
        var baseTooltip = new HoverTip(
            ModelDb.Power<StrengthPower>(),
            "Some test description",
            false
        );

        var newTooltip = baseTooltip with { Id = "CardTracker_CustomTip", IsSmart = false };

        var traverse = Traverse.Create(newTooltip);

        traverse.Property("Title").SetValue(title);
        traverse.Property("Description").SetValue(description);

        traverse.Property("Icon").SetValue(PreloadManager.Cache.GetTexture2D(GetIconPathForSetupCardType(iconType)));

        newTooltip = traverse.GetValue<HoverTip>();
        newTooltip.IsDebuff = isDebuff;
        newTooltip.Id = $"CardTracker_CustomTip_{title.GetHashCode()}";
        ModLog.Info(null, $"Created new hover tip: {newTooltip}.\nWith Icon: {newTooltip.Icon?.ResourcePath ?? "null"}");
        return newTooltip;
    }

    private static string GetIconPathForSetupCardType(SetupCardType cardType)
    {
        return cardType switch
        {
            SetupCardType.Offensive => NormalAttackIcon,
            SetupCardType.Defensive => DefenseIcon,
            SetupCardType.Buff => BuffIcon,
            SetupCardType.Debuff => DebuffIcon,
            SetupCardType.Mystery => MysteryIcon,
            _ => throw new ArgumentOutOfRangeException(nameof(cardType), cardType, null)
        };
    }
}