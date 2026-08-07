using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using TheScaled.TheScaledCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using TheScaled.TheScaledCode.Cards;
using TheScaled.TheScaledCode.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace TheScaled.TheScaledCode.Character;

  
  
  
  
public class TheScaled : PlaceholderCharacterModel
{
    public const string CharacterId = "TheScaled";
    
    public static readonly Color Color = new("2E2A4B");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;

    public override bool HideFromVanillaCharacterSelect => false;

    
    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeScaled>(),
        ModelDb.Card<StrikeScaled>(),
        ModelDb.Card<StrikeScaled>(),
        ModelDb.Card<StrikeScaled>(),
        ModelDb.Card<DefendScaled>(),
        ModelDb.Card<DefendScaled>(),
        ModelDb.Card<DefendScaled>(),
        ModelDb.Card<DefendScaled>(),
        ModelDb.Card<Chomp>(),
        ModelDb.Card<Respite>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<NaturalInstinct>()
    ];
    
    public override CardPoolModel CardPool => ModelDb.CardPool<TheScaledCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheScaledRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheScaledPotionPool>();
    
    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets. 
        These are just some of the simplest assets, given some placeholders to differentiate your character with. 
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override NCreatureVisuals? CreateCustomVisuals()
    {
        return NodeFactory<NCreatureVisuals>.CreateFromScene("res://TheScaled/scenes/character/character.tscn");
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
    public override string CustomEnergyCounterPath => "res://TheScaled/scenes/combat/energy_counters/scaled_energy_counter.tscn";
    public override string CustomCharacterSelectBg => "res://TheScaled/scenes/char_select/char_select_bg_scaled.tscn";
}