using System;
using SplashKitSDK;

public class Heal : Move
{
    private int _healAmount;

    public Heal(Character Caster, string type)
    {
        _Caster = Caster;
        if (type == "Potion")
        {
            _healAmount = 30;
        }
        else
        {
            _healAmount = _Caster.Level * (4 + SplashKit.Rnd(6));
        }
    }

    public override void Execute()
    {
        if(_Caster.Health + _healAmount > _Caster.MaxHealth)
        {
            _healAmount = _Caster.MaxHealth - _Caster.Health;
            _Caster.GiveHealth(_healAmount);
        }
        else
        {
            _Caster.GiveHealth(_healAmount);
        }
    }

    public override string Message()
    {
        return $"{_Caster.Name} healed for {_healAmount} health";
    }
}