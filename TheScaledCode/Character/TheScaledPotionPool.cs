using BaseLib.Abstracts;
using TheScaled.TheScaledCode.Extensions;
using Godot;

namespace TheScaled.TheScaledCode.Character;

public class TheScaledPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => TheScaled.Color;
    

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}