using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Helpers;
using TheScaled.TheScaledCode.Powers;
namespace TheScaled.TheScaledCode.Cards;

public abstract class SetupCard : TheScaledCard
{
    public enum SetupCardType
    {
        Offensive,
        Defensive,
        Buff,
        Debuff,
        Mystery,
        Stun
    }

    protected abstract SetupCardType CardSetupType { get; }
    protected IHoverTip AmbushHoverTip => HoverTipFactory.FromPower<SetupPower>();
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([AmbushHoverTip]);
    public virtual Dictionary<string, decimal> SetupData => new Dictionary<string, decimal>();
    protected SetupCard(int cost, CardType type, CardRarity rarity, TargetType target) : base(cost, type, rarity, target)
    {
        EventManager.OnSetupAdded += OnSetupPlayed;
        EventManager.OnAmbushTriggered += OnAmbushTriggered;
    }

    /// <summary>
    /// Adds a given setup to the target.
    /// </summary>
    /// <param name="choiceContext"></param>
    /// <param name="cardPlay"></param>
    /// <returns></returns>
    protected async Task AddSetup(CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        var ambush = GetAmbushPowerForApplier(base.Owner.Creature, cardPlay.Target);

        if (ambush is null)
        {
            ModLog.Info(this, $"Applying Ambush at 0 for enemy {cardPlay.Target.Name}");
            ambush = await PowerCmd.Apply<Ambush>(new ThrowingPlayerChoiceContext(), cardPlay.Target, 1, base.Owner.Creature, this);
            if (ambush != null)
            {
                ambush.SetAmount(0);
            }
            else
            {
                return;
            }
        }

        if (ambush is not null)
        {
            AmbushEntry entry;
            entry = new AmbushEntry(AmbushEffect, this);

            await ambush.AddAmbushEffect(entry);
        }
    }

    protected async Task AddSetupToAllEnemies()
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "wner.Creature.CombatState");

        var listOfEnemies = base.CombatState.Enemies;

        foreach (var enemy in listOfEnemies)
        {
            var ambush = GetAmbushPowerForApplier(base.Owner.Creature, enemy);

            if (ambush is null)
            {
                ModLog.Info(this, $"Applying Ambush at 0 for enemy {enemy.Name}");
                ambush = await PowerCmd.Apply<Ambush>(new ThrowingPlayerChoiceContext(), enemy, 1, base.Owner.Creature, this);
                if (ambush != null)
                {
                    ambush.SetAmount(0);
                }
                else
                {
                    return;
                }
            }
            AmbushEntry entry;
            entry = new AmbushEntry(AmbushEffect, this);

            if (ambush is not null)
            {
                await ambush.AddAmbushEffect(entry);
            }
        }
    }

    protected virtual void OnSetupPlayed(Player p)
    {
        return;
    }

    protected virtual void OnAmbushTriggered(Player p)
    {
        return;
    }

    public abstract Task AmbushEffect(AmbushMethodInfo info);

    public HoverTip GetHovertip()
    {
        var description = new LocString("cards", $"{base.Id.Entry.ToUpperInvariant()}.setup");
        foreach (var dynamicVar in base.DynamicVars)
        {
            description.Add(dynamicVar.Value);
        }
        return TooltipHelper.CreateSmartHoverTooltip($"Setup ({base.Title})", description, CardSetupType);
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