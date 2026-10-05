using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
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
        Mystery
    }

    protected abstract SetupCardType CardSetupType { get;}
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([AmbushHoverTip]);
    public virtual Dictionary<string,decimal> SetupData => new Dictionary<string, decimal>();
    protected SetupCard(int cost, CardType type, CardRarity rarity, TargetType target) : base(cost, type, rarity, target)
    {
    }

    
    /// <summary>
    /// Strips BBcode tags
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string GetCleanSetupText(string input)
    {
        //Strip out all BBCode style tags

        //Setup finding
        int setupIndex = input.IndexOf("[gold]Setup[/gold]:", StringComparison.OrdinalIgnoreCase);

        if (setupIndex == -1) //Look for ALL enemies version
        {
            setupIndex = input.IndexOf("[gold]Setup[/gold] ALL enemies:", StringComparison.OrdinalIgnoreCase);
        }

        if(setupIndex != -1)
        {
            var textWithoutSetup = input.Substring(setupIndex + "[gold]Setup[/gold]:".Length);
            int positionOfNewLine = textWithoutSetup.IndexOf("\n");
            if (positionOfNewLine == -1)
            {
                positionOfNewLine = textWithoutSetup.Length;
            }

            return textWithoutSetup.Substring(0, positionOfNewLine).Trim();
        }

        return string.Empty; // Return empty string if "Setup:" is not found
    }

    /// <summary>
    /// Adds a given setup to the target.
    /// </summary>
    /// <param name="choiceContext"></param>
    /// <param name="cardPlay"></param>
    /// <returns></returns>
    protected async Task AddSetup( CardPlay cardPlay, Dictionary<string,decimal>? data = null)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        var ambush = GetAmbushPowerForApplier(base.Owner.Creature, cardPlay.Target);

        if(ambush is null)
        {
            ModLog.Info(this,$"Applying Ambush at 0 for enemy {cardPlay.Target.Name}");
            ambush = await PowerCmd.Apply<Ambush>(new ThrowingPlayerChoiceContext(),cardPlay.Target,1,base.Owner.Creature,this);
            if(ambush != null)
            {
                ambush.SetAmount(0);
            } else
            {
                return;
            }
        }

        if (ambush is not null)
        {
            
            AmbushEntry entry;

            if(data is null)
            {
                entry = new AmbushEntry(AmbushEffect,this);
            } else
            {
                entry = new AmbushEntry(AmbushEffect,this, data);
            }
        
            await ambush.AddAmbushEffect(entry, GetHovertip(cardPlay.Target));
        }
    }



    protected async Task AddSetupToAllEnemies( Dictionary<string, decimal>? data = null )
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "wner.Creature.CombatState");

        var listOfEnemies = base.CombatState.Enemies;

        foreach (var enemy in listOfEnemies)
        {
            var ambush = GetAmbushPowerForApplier(base.Owner.Creature, enemy);

            if(ambush is null)
            {
                ModLog.Info(this,$"Applying Ambush at 0 for enemy {enemy.Name}");
            ambush = await PowerCmd.Apply<Ambush>(new ThrowingPlayerChoiceContext(),enemy,1,base.Owner.Creature,this);
            if(ambush != null)
            {
                ambush.SetAmount(0);
            } else
            {
                return;
            }
            }

            AmbushEntry entry;

            if (data is null)
            {
                entry = new AmbushEntry(AmbushEffect, this);
            }
            else
            {
                entry = new AmbushEntry(AmbushEffect, this, data);
            }

            if (ambush is not null)
            {
                await ambush.AddAmbushEffect(entry, GetHovertip(enemy));
            }
        }
    }

    public HoverTip GetHovertip(Creature target)
    {
        var description = GetCleanSetupText(GetDescriptionForPile(PileType.Hand, target));

        return TooltipHelper.CreateHoverTooltip($"Setup ({base.Title})", description, CardSetupType);
    }

    /// <summary>
    /// Specific override to replace the description manually, when we need to inject specific variables that won't be present on the card itself.
    /// </summary>
    /// <param name="descriptionOverride"></param>
    /// <returns></returns>
    public HoverTip GetHovertip(string descriptionOverride)
    {
        return TooltipHelper.CreateHoverTooltip($"Setup ({base.Title})", descriptionOverride, CardSetupType);
    }


    public abstract Task AmbushEffect(AmbushMethodInfo info, Dictionary<string,decimal> data);

    protected IHoverTip AmbushHoverTip => HoverTipFactory.FromPower<SetupPower>();

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