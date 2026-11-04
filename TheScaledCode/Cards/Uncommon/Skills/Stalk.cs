using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;
      
  
public class Stalk : TheScaledCard
{
    public Stalk() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [new PowerVar<DexterityPower>(4), new PowerVar<Ambush>(2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DexterityPower>(), HoverTipFactory.FromPower<Ambush>()];



    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, nameof(CombatState));
        await PowerCmd.Apply<DexterityPower>(choiceContext,Owner.Creature,DynamicVars["DexterityPower"].IntValue,Owner.Creature,this);
        await PowerCmd.Apply<Ambush>(choiceContext,CombatState.Enemies,DynamicVars["Ambush"].IntValue,Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3);
        base.DynamicVars["FrailPower"].UpgradeValueBy(1);
    }
}
