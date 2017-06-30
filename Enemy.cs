using System;
using SplashKitSDK;
using System.Collections.Generic;

public class Enemy : Character
{
    private Bitmap _Enemy;
    private List<Bitmap> _Enemies;
    private Bitmap _dragon;
    private Bitmap _mage;
    private Bitmap _lizardman;
    private Bitmap _ghost;
    private Bitmap _minotaur;
    
    public Enemy(int PlayerLevel)
    {
        _Enemies = new List<Bitmap>();
        _dragon = new Bitmap("Dragon", "dragon.png");
        _mage = new Bitmap("Mage", "badboy.png");
        _lizardman = new Bitmap("Lizard Man", "lizardman.png");
        _ghost = new Bitmap("Ghost", "ghost.png");
        _minotaur = new Bitmap("Minotaur", "minotaur.png");
        
        _Enemies.Add(_dragon);
        _Enemies.Add(_mage);
        _Enemies.Add(_lizardman);
        _Enemies.Add(_ghost);
        _Enemies.Add(_minotaur);

        _Enemy = _Enemies[SplashKit.Rnd(_Enemies.Count)];
        _Name = SplashKit.BitmapName(_Enemy);
        _Image = _Enemy;

        _XP = SplashKit.Rnd(PlayerLevel*100);
        _Level = 1 + _XP/100;
        _Health = _Level * 100;
        _MaxHealth = _Level * 100;
    
    }


    public override void Draw()
    {
        _HealthPercent = (double)_Health/_MaxHealth; 
        
        SplashKit.DrawBitmap(_Image, 800 - _Image.Width - _ImageBuffer, _ImageBuffer);
        SplashKit.FillRectangle(Color.DarkBlue, BoxOuterBuffer, BoxOuterBuffer, BoxWidth, BoxHeight);
        SplashKit.DrawRectangle(Color.White, BoxOuterBuffer, BoxOuterBuffer, BoxWidth, BoxHeight);
        SplashKit.DrawText($"{_Name} ", Color.White, MainFont, 40, BoxOuterBuffer + BoxInnerBuffer, BoxOuterBuffer + BoxInnerBuffer);
        SplashKit.DrawText($"Lv: {_Level} ", Color.White, MainFont, 40, BoxOuterBuffer + BoxWidth/2 + 50, BoxInnerBuffer + BoxOuterBuffer);
        SplashKit.DrawText($"HP:", Color.White, MainFont, 40, BoxOuterBuffer + BoxInnerBuffer, BoxOuterBuffer + BoxInnerBuffer + 40);
        SplashKit.FillRectangle(Color.Red, BoxOuterBuffer + 2*BoxInnerBuffer + 40, BoxOuterBuffer + 2*BoxInnerBuffer + 40, _HealthPercent * HealthBarWidth, HealthBarHeight);
        SplashKit.DrawRectangle(Color.White, BoxOuterBuffer + 2*BoxInnerBuffer + 40, BoxOuterBuffer + 2*BoxInnerBuffer + 40, BoxWidth - 2*BoxInnerBuffer - 2*BoxOuterBuffer, HealthBarHeight);
        

    }

    
}