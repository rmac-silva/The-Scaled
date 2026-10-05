using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Cards;

  
public sealed class ScaledAncient : TheScaledCard
{

	public ScaledAncient()
		: base(0, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		
	}

	protected override void OnUpgrade()
    {
        
    }
}