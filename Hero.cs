using System;
using SplashKitSDK;

public class Hero : Character
{
    private Window _GameWindow;
    private int _potionAmount;
    private int _XPGained;
    private int _EnemiesDefeated;

    public Hero(Window GameWindow)
    {
        _GameWindow = GameWindow;
        _potionAmount = 10;
        _Name = "Hero";
        _Image = new Bitmap("Hero", "hero.png");
        _XP = 100;
        _EnemiesDefeated = 0;

        BoxX = _GameWindow.Width - BoxWidth - BoxOuterBuffer;
        BoxY = _GameWindow.Height/4 * 3 - BoxHeight - BoxOuterBuffer;
        X = _ImageBuffer;
        Y = 3 * _GameWindow.Height/4 - _ImageBuffer - _Image.Height;

    
    }

    public int potionAmount
    {
        get 
        {
            return _potionAmount;
        }
    }

    public int EnemiesDefeated
    {
        get
        {
            return _EnemiesDefeated;
        }
    }

    public bool UsePotion()
    {
        if (_potionAmount > 0)
        {
            _potionAmount -= 1;
            return true;
        }
        return false;
        
    }

    public void IncrementEnemiesDefeated()
    {
        _EnemiesDefeated += 1;
    }

    public int ConsolidateStats(int EnemyLevel)
    {
        
        _XPGained = SplashKit.Rnd(100*(_Level/(_Level + 1 - EnemyLevel)));
        _XP += _XPGained;
        _Level = 1 + _XP/100;
        _MaxHealth = _Level * 100;
        _Health = (int)(_HealthPercent * _MaxHealth);
        return _XPGained;
        
    }

    public override void Draw()
    {   
        SplashKit.DrawBitmap(_Image, X, Y);
        
        SplashKit.FillRectangle(Color.DarkBlue, BoxX, BoxY, BoxWidth, BoxHeight);
        SplashKit.DrawRectangle(Color.White, BoxX, BoxY, BoxWidth, BoxHeight);
        
        _HealthPercent = (double)_Health/_MaxHealth; 



        SplashKit.DrawText($"{_Name} ", Color.White, MainFont, 40,_GameWindow.Width - BoxWidth - BoxOuterBuffer + BoxInnerBuffer, 3 * _GameWindow.Height/4 - BoxHeight - BoxOuterBuffer + BoxInnerBuffer);
        SplashKit.DrawText($"HP:", Color.White, MainFont, 40, _GameWindow.Width - BoxWidth - BoxOuterBuffer + BoxInnerBuffer, 3 * _GameWindow.Height/4 - BoxHeight - BoxOuterBuffer + BoxInnerBuffer + 40);
        SplashKit.DrawText($"Lv: {_Level}", Color.White, MainFont, 40, _GameWindow.Width - BoxOuterBuffer - BoxWidth/2 + 50, 3 * _GameWindow.Height/4 - BoxOuterBuffer - BoxHeight + BoxInnerBuffer);
        SplashKit.FillRectangle(Color.Red, _GameWindow.Width - BoxWidth - BoxOuterBuffer + 2*BoxInnerBuffer + 40, 3 * _GameWindow.Height/4 - BoxHeight - BoxOuterBuffer + 2*BoxInnerBuffer + 40, _HealthPercent * HealthBarWidth, HealthBarHeight);
        SplashKit.DrawRectangle(Color.White, _GameWindow.Width - BoxWidth - BoxOuterBuffer + 2*BoxInnerBuffer + 40, 3 * _GameWindow.Height/4 - BoxHeight - BoxOuterBuffer + 2*BoxInnerBuffer + 40, HealthBarWidth, HealthBarHeight);
        

    }

}