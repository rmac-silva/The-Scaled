using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using TheScaled.TheScaledCode.Powers;
namespace TheScaled.TheScaledCode.Cards;

public abstract class SetupCard : TheScaledCard
{
    protected IHoverTip AmbushHoverTip => HoverTipFactory.FromPower<Ambush>();
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([AmbushHoverTip]);
    protected SetupCard(int cost, CardType type, CardRarity rarity, TargetType target) : base(cost, type, rarity, target)
    {
    }

    public static Ambush? GetAmbushPowerForApplier(Creature owner, Creature c)
    {
        try
        {
            return c.Powers
            .OfType<Ambush>()
            .FirstOrDefault(power => power.Applier == owner);
        }
        catch (Exception)
        {

            return null;
        }
    }

}