using System;
using SplashKitSDK;

public abstract class Character
{
    protected int _XP;
    protected int _Level;
    protected Bitmap _Image;
    private bool _Countered;
    protected int _Health;
    protected int _MaxHealth;
    protected string _Name;
    protected double _HealthPercent;
    protected Font MainFont;
    protected int BoxX;
    protected int BoxY;
    protected int BoxWidth;
    protected int BoxHeight;
    protected int BoxInnerBuffer;
    protected int BoxOuterBuffer;
    protected int _ImageBuffer;
    protected int HealthBarWidth;
    protected int HealthBarHeight;
    protected int X;
    protected int Y;





    public Character()
    {
        _Level = 1 + _XP/100;
        _Health = _Level * 100;
        _MaxHealth =_Level * 100;
        _Countered = false;
        MainFont = new Font("MainFont", "pixels.ttf");

        BoxWidth = 350;
        BoxHeight = 100;
        BoxInnerBuffer = 10;
        BoxOuterBuffer = 25;
        _ImageBuffer = 40;
        HealthBarWidth = 280;
        HealthBarHeight = 20;

    }

    public double HealthPercent
    {  
        get
        {
            return Convert.ToDouble((double)_Health/(double)_MaxHealth); 
        }

    }


    public int Level
    {
        get
        {
            return _Level;
        }
    }

    public int XP
    {
        get
        {
            return _XP;
        }
    }

    public string Name
    {
        get
        {
            return _Name;
        }
    }

    public int Health
    {
        get
        {
            return _Health;
        }
    }

    public int MaxHealth
    {
        get
        {
            return _MaxHealth;
        }
    }

    public bool Countered
    {
        get
        {
            return _Countered;
        }
    }

    public abstract void Draw();

    public void TakeDamage(int damage)
    {
        if (_Health - damage < 0)
        {
            _Health = 0;
        }
        else
        {
            _Health -= damage;
        }
    }

    public void GiveHealth(int heal)
    {
        _Health += heal;
    }

    public void CounteredOn()
    {
        _Countered = true;
    }

    public void CounteredOff()
    {
        _Countered = false;
    }

}