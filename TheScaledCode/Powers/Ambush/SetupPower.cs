using BaseLib.Patches.UI;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using TheScaled.TheScaledCode.Helpers;

namespace TheScaled.TheScaledCode.Powers;

public class SetupPower : TheScaledPower
{
    public override PowerType Type => PowerType.None;
    public override bool AllowNegative => false;

    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier; //One per player, stacking
    private IEnumerable<HoverTip> HoverTipsForOwner => _extraHoverTips;

    public override LocString Title => getFormattedTitle();

    protected override IEnumerable<IHoverTip> ExtraHoverTips {
        get {
            

            var creatureHovertips = HoverTipsForOwner;
            
            //Uses SQL like LINQ grouping
            IEnumerable<IHoverTip> processedExtraTips = creatureHovertips
            .Where(tip => !string.IsNullOrEmpty(tip.Title))
            .GroupBy(tip => tip.Title)
            .Select(group =>
            {
                int count = group.Count();
                var first = group.First();

                // Append (Nx) if there are multiple identical tips
                string? displayTitle = count > 1 ? $"{first.Title} ({count}x)" : first.Title;
                
                if(string.IsNullOrEmpty(displayTitle))
                {
                    displayTitle = "Setup";
                }

                // Re-create a new HoverTip instance with the updated title
                return (IHoverTip)TooltipHelper.CreateHoverTooltip(displayTitle, first.Description, true, first.Icon);
            }).ToList();

            
            return processedExtraTips;
        }
        
    }

    public override int DisplayAmount
	{
		get
		{
			return HoverTipsForOwner.Count();
		}
	}

    private List<HoverTip> _extraHoverTips = [];

    /// <summary>
    /// Removes the setup power from the creature, including any extra hover tips that were added.
    /// This is called when an ambush is triggered or when the combat ends.
    /// </summary>
    public void RemoveSetup()
    {
        
        _extraHoverTips.Clear();
        base.RemoveInternal();

    }
    

    public void AddHovertip(HoverTip hoverTip)
    {
        
        _extraHoverTips.Add(hoverTip);

        InvokeDisplayAmountChanged();
    }

    public HoverTip GetHoverTip(string Title)
    {

        var res = HoverTipsForOwner.First( (HoverTip h) => h.Title == Title);
        return res;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        RemoveSetup();
        return Task.CompletedTask;
    }

    protected override void AfterCloned()
    {
        base.AfterCloned();
        _extraHoverTips = [];
    }

    

}