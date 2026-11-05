using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Cards;

namespace TheScaled.TheScaledCode.Powers;

public class NourishmentPower : TemporaryStrengthPower, ICustomModel
{
    private bool? _isPositive;

    public override AbstractModel OriginModel => ModelDb.Card<Nourishment>();
    protected override bool IsPositive => _isPositive ?? Owner?.IsPlayer ?? true;

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        _isPositive = target.IsPlayer;
        await base.BeforeApplied(target, amount, applier, cardSource);
    }
}