using System;
using SplashKitSDK;

public class Counter : Move
{
    public Counter(Character Caster)
    {
        _Caster = Caster;
    }

    public override void Execute()
    {
        _Caster.CounteredOn();
    }

    public override string Message()
    {
        return $"{_Caster.Name} has gotten into a countering stance";
    }

}