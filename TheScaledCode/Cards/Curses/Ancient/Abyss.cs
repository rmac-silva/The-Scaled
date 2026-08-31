using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace TheScaled.TheScaledCode.Cards;

[Pool(typeof(CurseCardPool))]
public class Abyss : CustomCardModel
{
    public Abyss() : base(0, CardType.Curse, CardRarity.Curse, TargetType.None)
    {
    }
    public override String CustomPortraitPath => "res://TheScaled/images/card_portraits/big/abyss.png";
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Eternal,CardKeyword.Ethereal,CardKeyword.Unplayable];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2)];

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        ArgumentNullException.ThrowIfNull(base.Owner.PlayerCombatState,"base.Owner.PlayerCombatState");
        if(base.Owner.PlayerCombatState.TurnNumber > 1)
        {
            return;
        }

        if(side == CombatSide.Player)
        {
            await CardPileCmd.Add(this,PileType.Discard);
        }
    }


    

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if(card != this)
        {
            return;
        }

        await PlayerCmd.LoseEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
    }
}