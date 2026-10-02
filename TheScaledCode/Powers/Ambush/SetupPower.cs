using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using TheScaled.TheScaledCode.Helpers;

namespace TheScaled.TheScaledCode.Powers;

public class SetupPower : TheScaledPower
{
    public override PowerType Type => PowerType.Debuff;
    public override bool AllowNegative => false;

    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier; //One per player, stacking
    private IEnumerable<HoverTip> HoverTipsForOwner => _extraHoverTips;
    protected override IEnumerable<IHoverTip> ExtraHoverTips {
        get {
            ModLog.Info(this,
                $"ExtraHoverTips requested. SetupHash={GetHashCode()}, Owner={base.Owner}, Applier={base.Applier}, StoredCount={_extraHoverTips.Count}");

            foreach (var tip in _extraHoverTips)
            {
                ModLog.Info(this,
                    $"Stored tooltip. SetupHash={GetHashCode()}, Title={tip.Title}, Id={tip.Id}, Description={tip.Description}");
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

            ModLog.Info(this,
                $"ExtraHoverTips returning {processedExtraTips.Count()} entries. SetupHash={GetHashCode()}, Owner={base.Owner}, Applier={base.Applier}");
            
            return processedExtraTips;
        }
        
    }

    public override int DisplayAmount
	{
		get
		{
            ModLog.Info(this,
                $"DisplayAmount requested: {_extraHoverTips.Count}. SetupHash={GetHashCode()}, Owner={base.Owner}, Applier={base.Applier}");
			return HoverTipsForOwner.Count();
		}
	}

    private readonly List<HoverTip> _extraHoverTips = [];

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
}