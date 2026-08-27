using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using TheScaled.TheScaledCode.Helpers;

namespace TheScaled.TheScaledCode.Powers;

public class SetupPower : TheScaledPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier; //One per player, stacking
    public int AmbushThresholdIncrease => _extraHoverTips.TryGetValue(base.Owner, out var tips) ? tips.Count() : 0;
    private IEnumerable<HoverTip> HoverTipsForOwner => _extraHoverTips.TryGetValue(base.Owner, out var tips) ? tips : Enumerable.Empty<HoverTip>();
    protected override IEnumerable<IHoverTip> ExtraHoverTips {
        get {
            var index = 0;
            foreach(var tip in _extraHoverTips)
            {
                ModLog.Info(this,$"Extra hover tip {index++} for {base.Owner}: {tip}");
            }

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

    protected Dictionary<Creature, IEnumerable<HoverTip>> _extraHoverTips = new Dictionary<Creature, IEnumerable<HoverTip>>();

    /// <summary>
    /// Removes the setup power from the creature, including any extra hover tips that were added.
    /// This is called when an ambush is triggered or when the combat ends.
    /// </summary>
    public void RemoveSetup()
    {
        
        _extraHoverTips.Remove(base.Owner);
        base.RemoveInternal();

    }
    public void AddHovertip(HoverTip hoverTip)
    {
        ModLog.Info(this,$"Adding hover tip {hoverTip} to {base.Owner}");
        _extraHoverTips[base.Owner] = _extraHoverTips.TryGetValue(base.Owner, out var existingTips) 
            ? existingTips.Append(hoverTip) 
            : new List<HoverTip> { hoverTip };
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        RemoveSetup();
        return Task.CompletedTask;
    }
}