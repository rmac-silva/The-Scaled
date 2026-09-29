using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheScaled.TheScaledCode.Powers;
using TheScaled.TheScaledCode.Powers.Cards;

namespace TheScaled.TheScaledCode.Cards;

public class Advantage : TheScaledCard
{
    public Advantage() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SetupPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await PowerCmd.Apply<AdvantagePower>(choiceContext, base.Owner.Creature,1,base.Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        Keywords.AddItem(CardKeyword.Innate);
    }
}