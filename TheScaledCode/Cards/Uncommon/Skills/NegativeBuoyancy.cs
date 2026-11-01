using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheScaled.TheScaledCode.Powers.ReusablePowers;

namespace TheScaled.TheScaledCode.Cards;

  
public class NegativeBuoyancy : TheScaledCard
{
    public NegativeBuoyancy()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    protected override bool ShouldGlowGoldInternal => base.CombatState?.HittableEnemies.Any((Creature e) => e.HasPower<DrownedPower>()) ?? false;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrownedPower>(4)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<DrownedPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        NDrownImpactVfx? child = NDrownImpactVfx.Create(cardPlay.Target!);
        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
        await PowerCmd.Apply<DrownedPower>(choiceContext, cardPlay.Target!, base.DynamicVars["DrownedPower"].IntValue, Owner.Creature,this);
        await PowerCmd.Apply<PersistDrownedPower>(choiceContext, cardPlay.Target!, 1, Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    
}
