using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace DavidSpaceShooter
{
   static class GameElements
    {
        
        static Player player;
        static Player2 player2;
        static List<Enemy> enemies;
        static PrintText printText;
        static List<GoldCoin> goldCoins;
        static Texture2D goldCoinSprite;
        static Background background;
        static HighScore highScore;

        public enum State { Menu, Run, HighScore,AddHS, Quit };

        public static State currentState;
        static Menu menu;

        public static void Initialize()
        {
            goldCoins = new List<GoldCoin>();
        }

       

        public static void LoadContent(ContentManager content, GameWindow window)
        {
            menu = new Menu((int)State.Menu);
            menu.AddItem(content.Load<Texture2D>("start"), (int)State.Run);
            menu.AddItem(content.Load<Texture2D>("highscore"), (int)State.HighScore);
            menu.AddItem(content.Load<Texture2D>("exit"), (int)State.Quit);

            background = new Background(content.Load<Texture2D>("level"), window);

            player = new Player(content.Load<Texture2D>("Sanic"), 380, 400, 2.5f, 4.5f, content.Load<Texture2D>("shot"));
            player2 = new Player2(content.Load<Texture2D>("Sanic2"), 580, 600, 2.5f, 4.5f, content.Load<Texture2D>("bullet"));
            goldCoinSprite = content.Load<Texture2D>("coin");


            GenerateEnemies(content, window);

            printText = new PrintText(content.Load<SpriteFont>("myFont"));

            

        }
        public static State MenuUpdate(GameTime gameTime)
        {
            return (State)menu.Update(gameTime);
        }
        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            menu.Draw(spriteBatch);
        }
        public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime)
        {
            
            background.Update(window);
            player.Update(window, gameTime);
            player2.Update(window, gameTime);
            if (enemies.Count == 0)
            {
                GenerateEnemies(content, window);

            }
            Random random = new Random();
            int newCoin = random.Next(1, 200);
            if (newCoin == 1)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - goldCoinSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height - goldCoinSprite.Height);
                goldCoins.Add(new GoldCoin(goldCoinSprite, rndX, rndY, gameTime));
            }

            foreach (GoldCoin gc in goldCoins.ToList())
            {
                if (gc.IsAlive)
                {
                    gc.Update(gameTime);
                    if (gc.CheckCollision(player) || gc.CheckCollision(player2))
                    {
                        goldCoins.Remove(gc);
                        player.Points++;
                        player2.Points++;
                    }
                }
                else
                    goldCoins.Remove(gc);
            }

            foreach (Enemy e in enemies.ToList())
            {
                foreach (Bullet b in player.Bullets)
                {
                    if (e.CheckCollision(b))
                    {
                        e.IsAlive = false;
                        player.Points++;
                    }
                }
                if (e.IsAlive)
                {
                    if (e.CheckCollision(player))
                        player.IsAlive = false;
                    e.Update(window);
                }
                else enemies.Remove(e);
            }
            foreach (Enemy e in enemies.ToList())
            {
                foreach (Bullet2 bs in player2.Bulletss)
                {
                    if (e.CheckCollision(bs))
                    {
                        e.IsAlive = false;
                        player.Points++;
                    }
                }
                if (e.IsAlive)
                {
                    if (e.CheckCollision(player2))
                        player2.IsAlive = false;
                    e.Update(window);
                }
                else enemies.Remove(e);
            }
       



            if (!player.IsAlive || !player2.IsAlive)
            {

                //TL Du anropar denna i AddHSUpdate()               Reset(window, content);
                return State.AddHS;
            }
           
            

            if (!player.IsAlive && !player2.IsAlive)
                return State.Menu;
            return State.Run;

            

        }

        public static void GenerateEnemies(ContentManager content, GameWindow window)
        {

            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("mine");
            for (int i = 0; i < 10; i++)
            {

                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);

                Mine temp = new Mine(tmpSprite, rndX, -rndY);
                
                enemies.Add(temp);
            }

            tmpSprite = content.Load<Texture2D>("tripod");
            for (int i = 0; i < 10; i++)
            {

                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 5);

                
                Tripod temp = new Tripod(tmpSprite, rndX, -rndY);

                enemies.Add(temp);

            }
            SpriteFont tmpFont = content.Load<SpriteFont>("myFont");
            printText = new PrintText(tmpFont);
            highScore = new HighScore(5, tmpFont);
            highScore.LoadFromFile("highscore.txt");
            return;
        }
        private static void Reset(GameWindow window, ContentManager content)
        {
            player.Reset(380, 400, 2.5f, 4.5f);
            player2.Reset(580, 600, 2.5f, 4.5f);
            enemies.Clear();
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("mine");
            for (int i = 0; i < 10; i++)
            {

                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                Mine temp = new Mine(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            tmpSprite = content.Load<Texture2D>("tripod");
            for (int i = 0; i < 10; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 5);
                Tripod temp = new Tripod(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
        }

        public static void RunDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            player.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            foreach (GoldCoin gc in goldCoins)
                gc.Draw(spriteBatch);
            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);
            printText.Print("Points: " + player.Points, spriteBatch, 0, 20);
        }
        public static State AddHSUpdate(GameTime gameTime, GameWindow window, ContentManager content)
        {
            if (highScore.EnterUpdate(gameTime, player.Points))
            {
                highScore.SaveToFile("highscore.txt");
                Reset(window, content);
                return State.HighScore;
            }
            return State.AddHS;
        }
        public static void AddHSDraw(SpriteBatch spriteBatch)
        {
            highScore.EnterDraw(spriteBatch);
        }
        public static State HighScoreUpdate()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                return State.Menu;
            return State.HighScore;
        }
        public static void HighScoreDraw(SpriteBatch spriteBatch)
        {
            highScore.PrintDraw(spriteBatch);
        }

       
        
    }
}
