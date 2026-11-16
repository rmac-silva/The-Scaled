using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
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
    public static readonly string Stun = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/intent_stun.tres");

    /// <summary>
    /// Creates a new HoverTip instance with the specified title, description, debuff and possible Texture2D Icon.
    /// Useful for recreating existing HoverTips with a new title
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="isDebuff"></param>
    /// <param name="icon"></param>
    /// <returns></returns>
    public static HoverTip CreateHoverTooltip(string title, string description, bool isDebuff = false, Texture2D? icon = null)
    {
        var baseTooltip = new HoverTip(
            ModelDb.Power<StrengthPower>(),
            "Some test description",
            false
        );

        var traverse = Traverse.Create(baseTooltip);

        traverse.Field("<Title>k__BackingField").SetValue(title);
        traverse.Field("<Description>k__BackingField").SetValue(description);
        traverse.Field("<Icon>k__BackingField").SetValue(icon);
        traverse.Field("<IsDebuff>k__BackingField").SetValue(isDebuff);
        traverse.Field("<Id>k__BackingField").SetValue($"CardTracker_CustomTip_{title.GetHashCode()}");
        traverse.Field("<IsSmart>k__BackingField").SetValue(false);

        return traverse.GetValue<HoverTip>();
    }

    public static HoverTip CreateSmartHoverTooltip(
        string title,
        LocString description,
        SetupCardType iconType
        )
    {
        var tooltip = new HoverTip(
            new LocString("cards","THESCALED-STRIKE_SCALED.title"),
            description
        );

        var traverse = Traverse.Create(tooltip);
        traverse.Field("<Title>k__BackingField").SetValue(title);
        traverse.Field("<Icon>k__BackingField").SetValue(
            PreloadManager.Cache.GetTexture2D(GetIconPathForSetupCardType(iconType))
        );
        traverse.Field("<Id>k__BackingField").SetValue($"CardTracker_SetupTooltip_{title.GetHashCode()}");
        traverse.Field("<IsSmart>k__BackingField").SetValue(true);

        return traverse.GetValue<HoverTip>();
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
            SetupCardType.Stun => Stun,
            _ => throw new ArgumentOutOfRangeException(nameof(cardType), cardType, null)
        };
    }
}