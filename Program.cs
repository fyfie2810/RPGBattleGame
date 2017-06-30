using System;
using SplashKitSDK;


    class Program
    {

        static void Main(string[] args)
        {
            Window gameWindow = new Window("Battle Game", 800, 600);
            BattleGame game = new BattleGame(gameWindow);

            do
            {
                gameWindow.Clear(Color.White);
                SplashKit.ProcessEvents();
                game.HandleInput();
                game.Draw();
                gameWindow.Refresh(60);
                
            }
            while(!gameWindow.CloseRequested && game.Quit != true);
            gameWindow.Close();
        }

    }

