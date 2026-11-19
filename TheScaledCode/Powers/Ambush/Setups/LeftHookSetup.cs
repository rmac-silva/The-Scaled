using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;




  
  
public class LeftHookSetup : BaseSetupPower
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new BoolVar("UpgradedHook",false)];
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    public override PowerStackType StackType => PowerStackType.Single;
    private CardModel? _cardModel;
    public override async Task TriggerSetupWithNoReset(PlayerChoiceContext ctx)
    {
        if(_cardModel == null) { return; }

        List<CardModel> generatedCards = [];
        for(int i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            CardModel card = _cardModel.CreateClone();
            generatedCards.Add(card);
        }

        await CardPileCmd.AddGeneratedCardsToCombat(generatedCards,PileType.Hand,Applier!.Player);
    }

    public void AddCardModel(CardModel cardModel)
    {
        ExtraHoverTips.Append(HoverTipFactory.FromCard(cardModel));   
        ((BoolVar)DynamicVars["UpgradedHook"]).BoolVal = cardModel.IsUpgraded;
    }

    protected override void AfterCloned()
    {
        _cardModel = null;
    }
}