

using System.Text.RegularExpressions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Events;
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
        ModLog.Info(null,$"[SetupCard.cs] Input for Setup Text Cleaning: {input}");

        //Setup finding
        int setupIndex = input.IndexOf("[gold]Setup[/gold]:", StringComparison.OrdinalIgnoreCase);

        if (setupIndex != -1)
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
    protected async Task AddSetup(PlayerChoiceContext choiceContext, CardPlay cardPlay, Dictionary<string,decimal>? data = null)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        var ambush = cardPlay.Target.GetPower<Ambush>();

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
        
            await ambush.AddAmbushEffect(entry, GetHovertip(cardPlay));
        }
    }



    protected async Task AddSetupToAllEnemies(PlayerChoiceContext choiceContext, CardPlay cardPlay, Dictionary<string, decimal>? data = null)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "wner.Creature.CombatState");

        var listOfEnemies = base.CombatState.Enemies;

        foreach (var enemy in listOfEnemies)
        {
            var ambush = enemy.GetPower<Ambush>();

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
                await ambush.AddAmbushEffect(entry, GetHovertip(cardPlay));
            }
        }
    }

    public HoverTip GetHovertip(CardPlay? cardPlay)
    {
        var description = GetCleanSetupText(GetDescriptionForPile(PileType.Hand, cardPlay?.Target));

        /*ModLog.Info(this, $"Setup Card Description: {description}");*/
        return TooltipHelper.CreateHoverTooltip($"Setup ({base.Title})", description, CardSetupType);
    }

    public HoverTip GetHovertip(CardPlay? cardPlay, string descriptionOverride)
    {
        /*ModLog.Info(this, $"Setup Card Description: {description}");*/
        return TooltipHelper.CreateHoverTooltip($"Setup ({base.Title})", descriptionOverride, CardSetupType);
    }


    protected abstract Task AmbushEffect(AmbushMethodInfo info, Dictionary<string,decimal> data);

    protected IHoverTip AmbushHoverTip => HoverTipFactory.FromPower<SetupPower>();
}