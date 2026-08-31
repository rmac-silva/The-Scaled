using BaseLib.Abstracts;
using TheScaled.TheScaledCode.Extensions;
using Godot;

namespace TheScaled.TheScaledCode.Character;

public class TheScaledCardPool : CustomCardPoolModel
{
    public override string Title => TheScaled.CharacterId; //This is not a display name.
    
    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 240f/360f; //Hue; changes the color.
    public override float S => 84f/100f; //Saturation
    public override float V => 33f/100f; //Brightness
    
    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load TheScaled/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override Color DeckEntryCardColor => new("ffffff");
    
    public override bool IsColorless => false;
}