using MegaCrit.Sts2.Core.Entities.Powers;

namespace TheScaled.TheScaledCode.Powers;

  
public class SmotheringPresencePower : TheScaledPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;
}