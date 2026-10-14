using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheScaled.TheScaledCode.Cards;

  
  
public class MudBath : TheScaledCard
{
    public MudBath() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Mud>()];

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int num = IsUpgraded ? ResolveEnergyXValue() + 1 : ResolveEnergyXValue();

        int blockAmount = base.DynamicVars.Block.IntValue * num;
        await CreatureCmd.GainBlock(Owner.Creature,blockAmount,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move,cardPlay);

        await Mud.AddMudCard(PileType.Draw,num,Owner);

    }


}

