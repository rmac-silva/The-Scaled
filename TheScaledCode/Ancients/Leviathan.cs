using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Relics.Ancient;

namespace TheScaled.TheScaledCode.Ancients;

  
public class LeviathanAncient : CustomAncientModel
{
    public override string? CustomScenePath => "res://TheScaled/scenes/events/background_scenes/leviathan.tscn";
    public override string? CustomMapIconPath => "res://TheScaled/images/map/ancients/ancient_node_leviathan.png";
    public override string? CustomMapIconOutlinePath => "res://TheScaled/images/map/ancients/ancient_node_leviathan_outline.png";
    public override string? CustomRunHistoryIconPath => "res://TheScaled/images/ui/run_history/leviathan.png";
    public override string? CustomRunHistoryIconOutlinePath => "res://TheScaled/images/ui/run_history/leviathan_outline.png";
    //Only valid on act 3
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 3;
    }
    protected override OptionPools MakeOptionPools => CreateOptionPools();

    // public override bool ShouldForceSpawn(ActModel act, AncientEventModel? rngChosenAncient)
    // {
    //     if(act.ActNumber() == 3)
    //     {
    //         return true;
    //     } else
    //     {
    //         return false;
    //     }
    // }

    private static OptionPools CreateOptionPools()
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		List<AncientOption> list = new List<AncientOption>
		{
            AncientOption<AceOfCups>(1, null, null),
			AncientOption<RoyalFlush>(1, null, null),
			AncientOption<SaltBandages>(1, null, null),
		};
		List<AncientOption> list2 = new List<AncientOption>
		{
            AncientOption<AbhorrentMass>(1, null, null),
			AncientOption<SeaUrchin>(1, null, null),
			AncientOption<RottenDiaphragm>(1, null, null),
		};
		List<AncientOption> list3 = new List<AncientOption>
		{
            AncientOption<EyeOfTheOracle>(1, null, null),
			AncientOption<HandOfTheKing>(1, null, null),
			AncientOption<HeartOfTheSea>(1, null, null),
		};
		
		
		
		return new OptionPools(MakePool(list.ToArray()), MakePool(list2.ToArray()), MakePool(list3.ToArray()));
	}

    public static CardModel[] GetAncientCards()
    {
        return new CardModel[]
        {
            ModelDb.Card<Excepcion>(),
            ModelDb.Card<Harrow>(),
            ModelDb.Card<MeteorHammer>(),
            ModelDb.Card<MidasTouch>(),
            ModelDb.Card<Motherlode>(),
            ModelDb.Card<RegalCleave>(),
            ModelDb.Card<ClimaxJumping>(),
            ModelDb.Card<Chosen>(),
            ModelDb.Card<Marshal>(),
            ModelDb.Card<Overwhelm>(),
            ModelDb.Card<Plagiarize>(),
            ModelDb.Card<PulsingLattice>(),
            ModelDb.Card<ThePromotion>(),
            ModelDb.Card<WitchesCauldron>(),


        };
    }

    
    
}