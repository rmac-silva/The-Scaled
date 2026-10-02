using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Afflictions;
using TheScaled.TheScaledCode.Powers.Cards;

namespace TheScaled.TheScaledCode.Cards;

  
public class Pursuit : TheScaledCard
{
    public Pursuit()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<PursuitPower>(2), new EnergyVar(1)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<PursuitPower>(),
            HoverTipFactory.FromKeyword(CardKeyword.Ethereal),
        ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //Apply Pursuit Power
        await PowerCmd.Apply<PursuitPower>(choiceContext,base.Owner.Creature,base.DynamicVars["PursuitPower"].IntValue,base.Owner.Creature,this);
    }

    protected override CardLocation GetResultLocationForCardPlay()
	{
		CardLocation resultLocationForCardPlay = base.GetResultLocationForCardPlay();
		if (resultLocationForCardPlay.pileType == PileType.Discard)
		{
			resultLocationForCardPlay.pileType = PileType.Draw;
			resultLocationForCardPlay.position = CardPilePosition.Top;
		}
		return resultLocationForCardPlay;
	}

    protected override void OnUpgrade()
    {
        
    }
}
