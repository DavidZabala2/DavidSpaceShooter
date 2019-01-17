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

        /* Olika gamestates  David -*/
        public enum State { Menu, Run, HighScore,AddHS, Quit };

        public static State currentState;
        static Menu menu;

        public static void Initialize()
        {
            goldCoins = new List<GoldCoin>();
        }

       

        public static void LoadContent(ContentManager content, GameWindow window) /* Laddar all content - David */
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
        public static State MenuUpdate(GameTime gameTime) /* Uppdatera Menyn och metoden under är för att rita ut backgrounden och meny - David */
        {
            return (State)menu.Update(gameTime);
        }
        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            menu.Draw(spriteBatch);
        }
        public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime) /* Uppdaterar allt medans man kör allt - David */
        {
            
            background.Update(window);
            player.Update(window, gameTime);
            player2.Update(window, gameTime);
            if (enemies.Count == 0) /* Om det inte finns några fiender så ska det komma flera - David*/
            {
                GenerateEnemies(content, window);

            }
            Random random = new Random(); /*  Slumpad position och slumpad coin. */
            int newCoin = random.Next(1, 200); //en chans på 200 - Fredrik
            if (newCoin == 1)
            { //Var ska guldmyntet uppstå - Fredrik
                int rndX = random.Next(0, window.ClientBounds.Width - goldCoinSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height - goldCoinSprite.Height);
                //Lägg till myntet i listan
                goldCoins.Add(new GoldCoin(goldCoinSprite, rndX, rndY, gameTime));
            }

            foreach (GoldCoin gc in goldCoins.ToList()) /*För varje goldcoin I listan så ska det kollas om coin lever och deras kollision med spelare sedan om kollision sker med spelare så ska det läggas till en poäng*/
            {
                if (gc.IsAlive) //Kontrollera om guldmyntet lever - David
                {
                    gc.Update(gameTime); //gc.Update() kollar om guldmyntet ar blivit för gammalt för att få leva vidare.
                    if (gc.CheckCollision(player) || gc.CheckCollision(player2)) //Kolliderar coin med spelare
                    {
                        goldCoins.Remove(gc); //Ta bort mynt vid kollision
                        player.Points++; //Lägger till poäng till spelarna
                        player2.Points++;
                    }
                }
                else
                    goldCoins.Remove(gc);
            }
            //Gå igenom alla fiender
            foreach (Enemy e in enemies.ToList())
            {
                //Kontrollera om fienden kolliderar med ett skott
                foreach (Bullet b in player.Bullets)
                {
                    if (e.CheckCollision(b)) //Kollision uppstod
                    {
                        e.IsAlive = false; //Döda fienden
                        player.Points++; //Ge spelaren poäng
                    }
                }
                if (e.IsAlive) //Kontrollera om fienden lever - David
                {
                    if (e.CheckCollision(player)) //Kontrollera kollision med spelaren
                        player.IsAlive = false;
                    e.Update(window); //Flytta på dem
                }
                else enemies.Remove(e); //Ta bort fienden för den är död
            }
            foreach (Enemy e in enemies.ToList()) //Allt ovan fast för spelare 2
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
                else enemies.Remove(e); //Allt ovan fast för spelare 2
            }
       



            if (!player.IsAlive || !player2.IsAlive) //Spelarna är döda
            {

                
                return State.AddHS; //Återgå till att lägga till en highscore
            }
           
            

            if (!player.IsAlive && !player2.IsAlive) //Spelarna är döda
                return State.Menu; //Återgå till menyn
            return State.Run; //Stanna kvar i Run = Spelet

            
             
        }

        public static void GenerateEnemies(ContentManager content, GameWindow window)
        {
            //Skapa fiender och hur många - Fredrik
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

            //Skapa fiender
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("mine");
            for (int i = 0; i < 10; i++)
            {

                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                Mine temp = new Mine(tmpSprite, rndX, rndY);
                enemies.Add(temp); //Lägg till i listan
            }
            tmpSprite = content.Load<Texture2D>("tripod");
            for (int i = 0; i < 10; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 5);
                Tripod temp = new Tripod(tmpSprite, rndX, rndY);
                enemies.Add(temp); //Lägg till i listan
            }
        }

        public static void RunDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch); //Ritar ut allt - David
            player.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            foreach (GoldCoin gc in goldCoins) //Varje coin i listan ska ritas ut.
                gc.Draw(spriteBatch);
            foreach (Enemy e in enemies) //Varje enemy i listan ska ritas ut.
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
