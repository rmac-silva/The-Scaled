using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;



  

  
  
public class HuntersMark : SetupCard
{
    public HuntersMark() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4m, ValueProp.Move),
		new DynamicVar("AmbushEffect",15m)
        ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [AmbushHoverTip,HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    protected override SetupCardType CardSetupType => SetupCardType.Offensive;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        
        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await base.AddSetup(cardPlay);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }

    public override async Task AmbushEffect(AmbushMethodInfo info, Dictionary<string, decimal> data)
    {
        if(info.applier is null)
        {
            ModLog.Warning(this,"Tried applying an ambush effect that depends on info.applier, when it is null.");
            return;
        }

        await CreatureCmd.Damage(info.choiceContext,info.target,base.DynamicVars["AmbushEffect"].IntValue,ValueProp.Move,info.applier);
        
        // Add a new setup effect with increased damage
        var ambushPower = GetAmbushPowerForApplier(base.Owner.Creature,info.target);

        if (ambushPower != null)
        {
            var newEntry = new AmbushEntry(this.AmbushEffect, this, data);
            await ambushPower.AddAmbushEffect(newEntry, GetHovertip(info.target));
        }

        return;
    }
}