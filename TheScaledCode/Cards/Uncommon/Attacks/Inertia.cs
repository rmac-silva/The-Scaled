using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheScaled.TheScaledCode.Powers;

namespace TheScaled.TheScaledCode.Cards;

public class Inertia : SetupCard
{
    public Inertia() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

    protected override SetupCardType CardSetupType => SetupCardType.Offensive;



    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd
            .Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

    }

    protected override async Task AmbushEffect(AmbushMethodInfo info, Dictionary<string, decimal> data)
    {
        if (info.choiceContext is null)
        {
            ModLog.Warning(this, "ChoiceContext for AmbushMethodInfo was null, which should not happen.");
            return;
        }

        if (info.target is null)
        {
            ModLog.Warning(this, "Target for AmbushMethodInfo was null, which should not happen.");
            return;
        }

        if (info.applier is null)
        {
            ModLog.Warning(this, "Applier for AmbushMethodInfo was null, which should not happen.");
            return;
        }

        // Safely check for data and perform a single dictionary lookup
        if (data != null && data.TryGetValue("damage", out decimal damage))
        {
            await CreatureCmd.Damage(
                info.choiceContext,
                info.target,
                damage,
                MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered,
                info.applier
            );

            // Add a new setup effect with increased damage
            var ambushPower = info.target.GetPower<Ambush>();

            if (ambushPower != null)
            {
                var newData = new Dictionary<string, decimal>(1)
                {
                    ["damage"] = damage * 1.5m
                };

                var newEntry = new AmbushEntry(this.AmbushEffect, this, newData);
                await ambushPower.AddAmbushEffect(newEntry, GetHovertip(null));
            }
        }
        else
        {
            ModLog.Error(
                this,
                "Inertia needs parameter \"damage\" to be defined in its data sources. But none was found!",
                new KeyNotFoundException("No key 'damage' found for Inertia's data Dictionary.")
            );
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}