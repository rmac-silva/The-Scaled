

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

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        var ambush = cardPlay.Target.GetPower<Ambush>();

        if (ambush is not null)
        {
            var description = GetCleanSetupText(GetDescriptionForPile(PileType.Hand,cardPlay.Target));
            ModLog.Info(this, $"Setup Card Description: {description}");

            AmbushEntry entry = new AmbushEntry(AmbushEffect,this);
        
            await ambush.AddAmbushEffect(entry, TooltipHelper.CreateHoverTooltip($"Setup ({base.Title})", description, CardSetupType));
        }
    }

    protected async Task ApplyAmbushToAllEnemies(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "wner.Creature.CombatState");

        var listOfEnemies = base.CombatState.Enemies;

        var description = GetCleanSetupText(GetDescriptionForPile(PileType.Hand,cardPlay.Target));
        ModLog.Info(this, $"Setup Card Description: {description}");

        foreach (var enemy in listOfEnemies)
        {
            var ambush = enemy.GetPower<Ambush>();

            AmbushEntry entry = new AmbushEntry(AmbushEffect,this);

            if (ambush is not null)
            {
                await ambush.AddAmbushEffect(entry, TooltipHelper.CreateHoverTooltip($"Setup ({base.Title})", description, CardSetupType));
            }
        }
    }


    protected abstract Task AmbushEffect(AmbushMethodInfo info);

    protected IHoverTip AmbushHoverTip => HoverTipFactory.FromPower<SetupPower>();
}