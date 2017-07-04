using System;
using SplashKitSDK;
using System.Collections.Generic;

public class BattleGame
{
    private bool _Quit = false;
    private bool _InstructionsToggle;
    private Bitmap _Background;
    private Hero _Player;
    private Enemy _Enemy;
    private Window gameWindow;
    private List<Button> _MainMenu;
    private List<Button> _AttackMenu;
    private List<Button> _HealMenu;
    private List<Button> _VolMenu;
    private Menu _ActiveMenu;
    private Font _MainFont;
    private int _InstructionsX;
    private int _InstructionsY;
    private int _LineHeight;
    private int _CurrentLevel;

    // main menu buttons
    private Button _AttackButton;
    private Button _HealButton;
    private Button _CounterButton;
    private Button _QuitButton;
    
    // attack menu buttons
    private Button _ChargeButton;
    private Button _SlashButton;
    private Button _SpellButton;
    private Button _AttackBackButton;

    // heal menu buttons
    private Button _PotionButton;
    private Button _MagicButton;
    private Button _HealBackButton;
    private Button _EmptyButton;

    // dialogue box
    private Button _MessageBox;
    private Button _InstructionsCloseButton;

    //volumebuttons
    private Button _VolumeUp;
    private Button _VolumeDown;

    //sounds
    private SoundEffect _AttackSound;
    private SoundEffect _HealSound;
    private Music _Music;
    private float _soundLevel;






    public BattleGame(Window _GameWindow)
    {
        

        //Game Assets
        gameWindow = _GameWindow;
        _Background = new Bitmap("Background", "background1.png");
        _AttackSound = new SoundEffect("Attack", "Attack.wav");
        _HealSound = new SoundEffect("Heal", "Heal.wav");
        _Music = new Music("GameMusic", "Music.mp3");
        _MainFont = new Font("MainFont", "pixels.ttf");
        _InstructionsToggle = true;
        _InstructionsX = gameWindow.Width/8;
        _InstructionsY = gameWindow.Height/8;
        _LineHeight = 30;

        //Game Objects
        _Player = new Hero(gameWindow);
        _Enemy = new Enemy(1);
        _MainMenu = new List<Button>();
        _AttackMenu = new List<Button>();
        _HealMenu = new List<Button>();
        _VolMenu = new List<Button>();
        _MessageBox = new Button { X =0, Y = 0, Caption = "", Width = 0, Height = 0};
        _ActiveMenu = Menu.MainMenu;
        _soundLevel = 0.5F;

        CreateMainMenu();
        CreateAttackMenu();
        CreateHealMenu();
        CreateVolButtons();

        //Main Buttons
        _AttackButton.OnClick += (btn) => _ActiveMenu = Menu.AttackMenu;
        _HealButton.OnClick += (btn) => _ActiveMenu = Menu.HealMenu;
        _CounterButton.OnClick += (btn) =>  Counter(_Player);
        _QuitButton.OnClick += (btn) => _Quit = true;

        //Attack Buttons
        _ChargeButton.OnClick += (btn) => Charge(_Player, _Enemy);
        _SlashButton.OnClick += (btn) => Slash(_Player, _Enemy);
        _SpellButton.OnClick += (btn) => Spell(_Player, _Enemy);
        _AttackBackButton.OnClick += (btn) =>  _ActiveMenu = Menu.MainMenu;
        
        //HealButtons

        _PotionButton.OnClick += (btn) => PotionHeal(_Player);
        _MagicButton.OnClick += (btn) => MagicHeal(_Player);
        _HealBackButton.OnClick += (btn) =>  _ActiveMenu = Menu.MainMenu;

        //Volume Buttons
        _VolumeUp.OnClick += (btn) =>  ChangeVolume(0.1F);
        _VolumeDown.OnClick += (btn) =>  ChangeVolume(-0.1F);
        SplashKit.PlayMusic(_Music, 8, _soundLevel);

        //instructions button
        _InstructionsCloseButton = new Button { X = gameWindow.Width - 300, Y = 0, Caption = "Instructions", Width = 150, Height = 30};
        _InstructionsCloseButton.OnClick += (btn) => ToggleInstructions();
    }


    public enum Menu
    {
        MainMenu,
        AttackMenu,
        HealMenu,
        MessageBox
    }

    public bool Quit
    {
        get
        {
            return _Quit;
        }
    }

    public void HandleInput()
    {
        if (_InstructionsToggle == false)
        {

            switch(_ActiveMenu)
            {
                case Menu.MainMenu:
                    foreach (Button button in _MainMenu)
                    {
                        button.HandleInput();
                    }
                    break;
                case Menu.AttackMenu:
                    foreach (Button button in _AttackMenu)
                    {
                        button.HandleInput();
                    }
                    break;
                case Menu.HealMenu:
                    foreach (Button button in _HealMenu)
                    {
                        button.HandleInput();
                    }
                    break;
                case Menu.MessageBox:
                        _MessageBox.HandleInput();
                    break;
            }
        }
        _InstructionsCloseButton.HandleInput();

        foreach (Button button in _VolMenu)
        {
            button.HandleInput();
        }



    }
        
    public void Draw()
    {
        
        SplashKit.DrawBitmap(_Background, 0, 0);
        switch(_ActiveMenu)
        {
            case Menu.MainMenu:
                DrawMenu(_MainMenu);
                break;
            case Menu.AttackMenu:
                DrawMenu(_AttackMenu);
                break;
            case Menu.HealMenu:
                DrawMenu(_HealMenu);
                break;
            case Menu.MessageBox:
                _MessageBox.Draw();
                break;
        }

        _Player.Draw();
        _Enemy.Draw();
        DrawMenu(_VolMenu);
        DrawDefeatedBox();
        _InstructionsCloseButton.Draw();

        if (_InstructionsToggle){
            DrawInstructions();
        }
    }

    public void EnemyTurn()
    {
        _Enemy.CounteredOff();

        if(SplashKit.Rnd() < 0.5)
        {
            Slash(_Enemy, _Player);
        }
        else if(SplashKit.Rnd() > 0.5)
        {
            MagicHeal(_Enemy);
        }
        else
        {
            Counter(_Enemy);
        }
    }

    public bool CheckEnemyDead()
    {
        if(_Enemy.Health <= 0)
        {   
            return true;
        }
        return false;

    }

    public bool CheckPlayerDead()
    {
        if(_Player.Health <= 0)
        {
            return true;
        }
        return false;

    }

    public void NewEnemy()
    {
        _Enemy = new Enemy(_Player.Level);
    }

    public void Charge(Character _Caster, Character _Target)
    {
       Attack Attack = new Attack(_Caster, _Target, 6, 0.9);
       Attack.Execute();
       DisplayMessage(Attack.Message());
       DisplayBoxBehaviour(_Caster);
       SplashKit.PlaySoundEffect("Attack", _soundLevel);
       _Player.CounteredOff();
    }

    public void Slash(Character _Caster, Character _Target)
    {
       Attack Attack = new Attack(_Caster, _Target, 10, 0.7);
       Attack.Execute();
       DisplayMessage(Attack.Message());
       DisplayBoxBehaviour(_Caster);  
       SplashKit.PlaySoundEffect("Attack", _soundLevel);
       _Player.CounteredOff(); 
    }

    public void Spell(Character _Caster, Character _Target)
    {
       _Target.CounteredOff();
       Attack Attack = new Attack(_Caster, _Target, 2, 0.7);
       Attack.Execute();
       DisplayMessage(Attack.Message());
       DisplayBoxBehaviour(_Caster); 
       SplashKit.PlaySoundEffect("Attack", _soundLevel);
       _Player.CounteredOff();
    }

    public void MagicHeal(Character _Caster)
    {
       Heal Heal = new Heal(_Caster, "Magic");
       Heal.Execute();
       DisplayMessage(Heal.Message());
       DisplayBoxBehaviour(_Caster);
       SplashKit.PlaySoundEffect("Heal", _soundLevel);
       _Player.CounteredOff();
    }

    public void PotionHeal(Character _Caster)
    {
       Heal Heal = new Heal(_Caster, "Potion");
       if (_Player.UsePotion())
       {
            Heal.Execute();
            DisplayMessage(Heal.Message());
            DisplayBoxBehaviour(_Caster);
            _PotionButton.ChangeInput($"Potion ({_Player.potionAmount})");
            SplashKit.PlaySoundEffect("Heal", _soundLevel);
       }
       else
       {
           NoPotionsMessage();
       }
       _Player.CounteredOff();
       
    }

    public void Counter(Character _Caster)
    {
       Counter Counter = new Counter(_Caster);
       Counter.Execute();
       DisplayMessage(Counter.Message());
       DisplayBoxBehaviour(_Caster);
    }


    public void CreateMainMenu()
    {
        _AttackButton = new Button { X = 0, Y = 3* gameWindow.Height/4, Caption = "Attack", Width = 400, Height = 75};
        _HealButton = new Button { X = gameWindow.Width/2, Y = 3* gameWindow.Height/4, Caption = "Heal", Width = 400, Height = 75};
        _CounterButton = new Button { X = 0, Y = 3* gameWindow.Height/4 + 75, Caption = "Counter", Width = 400, Height = 75};
        _QuitButton = new Button { X = gameWindow.Width/2, Y = 3* gameWindow.Height/4 + 75, Caption = "Quit", Width = 400, Height = 75};

        _MainMenu.Add(_AttackButton);
        _MainMenu.Add(_HealButton);
        _MainMenu.Add(_CounterButton);
        _MainMenu.Add(_QuitButton);
    }

    public void CreateAttackMenu()
    {
        _ChargeButton = new Button { X = 0, Y = 3* gameWindow.Height/4, Caption = "Charge", Width = 400, Height = 75};
        _SlashButton = new Button { X = gameWindow.Width/2, Y = 3* gameWindow.Height/4, Caption = "Slash", Width = 400, Height = 75};
        _SpellButton = new Button { X = 0, Y = 3* gameWindow.Height/4 + 75, Caption = "Spell", Width = 400, Height = 75};
        _AttackBackButton = new Button { X = gameWindow.Width/2, Y = 3* gameWindow.Height/4 + 75, Caption = "Back", Width = 400, Height = 75};

        _AttackMenu.Add(_ChargeButton);
        _AttackMenu.Add(_SlashButton);
        _AttackMenu.Add(_SpellButton);
        _AttackMenu.Add(_AttackBackButton);
    }

    public void CreateHealMenu()
    {
        _PotionButton = new Button { X = 0, Y = 3* gameWindow.Height/4, Caption = $"Potion ({_Player.potionAmount})", Width = 400, Height = 75};
        _MagicButton = new Button { X = gameWindow.Width/2, Y = 3* gameWindow.Height/4, Caption = "Magic Heal", Width = 400, Height = 75};
        _HealBackButton = new Button { X = 0, Y = 3* gameWindow.Height/4 + 75, Caption = "Back", Width = 400, Height = 75};
        _EmptyButton = new Button { X = gameWindow.Width/2, Y = 3* gameWindow.Height/4 + 75, Caption = "", Width = 400, Height = 75};
        
        _HealMenu.Add(_PotionButton);
        _HealMenu.Add(_MagicButton);
        _HealMenu.Add(_HealBackButton);
        _HealMenu.Add(_EmptyButton);
    }

    public void CreateVolButtons()
    {
        _VolumeUp = new Button { X = gameWindow.Width - 75, Y = 0, Caption = "Vol +", Width = 75, Height = 30};
        _VolumeDown = new Button { X = gameWindow.Width - 2*75, Y = 0, Caption = "Vol -", Width = 75, Height = 30};

        _VolMenu.Add(_VolumeUp);
        _VolMenu.Add(_VolumeDown);
    }


    public void DrawMenu(List<Button> buttons)
    {
        foreach (Button button in buttons)
        {
            button.Draw();
        }
    }

    public void AfterTurn()
    {
       if(!CheckEnemyDead())
        {
            EnemyTurn();
        }
        else
        {
            
            _CurrentLevel = _Player.Level;
            DisplayMessage($"{_Enemy.Name} has fainted");
            _MessageBox.OnClick += (btn) => XpGain(_Player.ConsolidateStats(_Enemy.Level));
            _Player.IncrementEnemiesDefeated();
        }
        if(CheckPlayerDead())
        {
            DisplayMessage("Player dead");
            _Quit = true;
        }
    }

    public void DisplayMessage(string Message)
    {
        _MessageBox = new Button { X =0, Y = 3* gameWindow.Height/4, Caption = Message, Width = gameWindow.Width, Height = gameWindow.Height/3};
        _ActiveMenu = Menu.MessageBox;
    }

    public void DisplayBoxBehaviour(Character Caster)
    {       
            if (Caster == _Enemy)
                {
                    _MessageBox.OnClick += (btn) =>  _ActiveMenu = Menu.MainMenu;
                }
            else if (Caster == _Player)
                {
                    _MessageBox.OnClick += (btn) => AfterTurn();
                }
   
    }

    public void XpGain(int XP)
    {
        DisplayMessage($"{_Player.Name} gained {XP} XP");
        if (_Player.Level > _CurrentLevel)
        {
            _MessageBox.OnClick += (btn) => NewLevelGained();
        }
        else
        {
            _MessageBox.OnClick += (btn) => SpawnNewEnemy();
        }
    }

    public void SpawnNewEnemy()
    {
        NewEnemy();
        DisplayMessage($"A level {_Enemy.Level} {_Enemy.Name} has appeared");
        _MessageBox.OnClick += (btn) => _ActiveMenu = Menu.MainMenu;
    }

    public void NoPotionsMessage()
    {
        DisplayMessage("You have no potions left");
        _MessageBox.OnClick += (btn) => _ActiveMenu = Menu.MainMenu;
    }

    public void updatePotionsButton()
    {
        _PotionButton = new Button { X = 0, Y = 3* gameWindow.Height/4, Caption = $"Potion ({_Player.potionAmount})", Width = 400, Height = 75};
    }

    public void ChangeVolume(float amount)
    {
        if(!(_soundLevel + amount < 0) &&  !(_soundLevel + amount > 1))
        {
            _soundLevel += amount;
            SplashKit.SetMusicVolume(_soundLevel);
        }

    }

    public void DrawDefeatedBox()
    {
        SplashKit.FillRectangle(Color.DarkBlue, 0, 3 * gameWindow.Height/4-40, 400, 40);
        SplashKit.DrawRectangle(Color.White, 0, 3 * gameWindow.Height/4-40, 400, 40);
        SplashKit.DrawText($"Enemies Defeated: {_Player.EnemiesDefeated}", Color.White, _MainFont, 40, 40, 3 * gameWindow.Height/4 - 40);
    }

    public void NewLevelGained()
    {
        DisplayMessage($"{_Player.Name} is now level {_Player.Level}");
        _MessageBox.OnClick += (btn) => SpawnNewEnemy();
        
    }

    public void DrawInstructions()
    {
        

        SplashKit.FillRectangle(Color.DarkBlue, _InstructionsX, _InstructionsY, 3*gameWindow.Width/4, 3*gameWindow.Height/4);
        SplashKit.DrawRectangle(Color.White, _InstructionsX, _InstructionsY, 3*gameWindow.Width/4, 3*gameWindow.Height/4);
        SplashKit.DrawText($"Welcome to C# Battler", Color.White, _MainFont, 40, _InstructionsX + 20 , _InstructionsY);

        SplashKit.DrawText($"Use the menu to navigate", Color.White, _MainFont, 22, _InstructionsX + 20 , _InstructionsY + 2*_LineHeight);

        SplashKit.DrawText($"The Charge Attack is a mostly accurate but low damage attack", Color.White, _MainFont, 22, _InstructionsX + 20 , _InstructionsY + 4*_LineHeight);
        SplashKit.DrawText($"The Slash Attack is a semi accurate and high damage attack", Color.White, _MainFont, 22, _InstructionsX + 20 , _InstructionsY + 5*_LineHeight);
        SplashKit.DrawText($"The Spell Attack is a semi accurate low attack that ignores counters", Color.White, _MainFont, 22, _InstructionsX + 20 , _InstructionsY + 6*_LineHeight);
        
        SplashKit.DrawText("A counter is 50% likely to reflect back an attack for half damage", Color.White, _MainFont, 22, _InstructionsX + 20 , _InstructionsY + 8*_LineHeight);

        SplashKit.DrawText($"You begin with 10 potions which each heal 30 Health", Color.White, _MainFont, 22, _InstructionsX + 20 , _InstructionsY + 10*_LineHeight);
        SplashKit.DrawText($"A magic heal will heal a random amount relative to your level", Color.White, _MainFont, 22, _InstructionsX + 20 , _InstructionsY + 11*_LineHeight);
        SplashKit.DrawText("The button labelled Instructions in the top left will toggle this box", Color.White, _MainFont, 22, _InstructionsX + 20 , _InstructionsY + 13*_LineHeight);
        SplashKit.DrawText("Enjoy!", Color.White, _MainFont, 22, _InstructionsX + 20 , _InstructionsY + 14*_LineHeight);


    }


    public void ToggleInstructions()
    {
        if(_InstructionsToggle)
        {
            _InstructionsToggle = false;
        }
        else if(_InstructionsToggle == false)
        {
            _InstructionsToggle = true;
        }
    }




}


public delegate void ClickAction(Button btn);

