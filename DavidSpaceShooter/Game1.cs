using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DavidSpaceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        PrintText printText;
        List<Enemy> enemies;
        List<GoldCoin> goldCoins;
        Texture2D goldCoinSprite;   
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            goldCoins = new List<GoldCoin>();
           
            base.Initialize();
            

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player = new Player(Content.Load<Texture2D>("ship"), 380, 400, 2.5f, 4.5f, Content.Load<Texture2D>("bullet"));
            
            printText = new PrintText(Content.Load<SpriteFont>("myFont"));

            goldCoinSprite = Content.Load<Texture2D>("coin");

            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite = Content.Load<Texture2D>("mine");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, Window.ClientBounds.Height / 2);

                Mine temp = new Mine(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }

            tmpSprite = Content.Load<Texture2D>("tripod");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, Window.ClientBounds.Height / 2);

                Tripod temp = new Tripod(tmpSprite, rndX, rndY);

                enemies.Add(temp);
            }
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            player.Update(Window, gameTime);
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
                        this.Exit();
                    e.Update(Window);
                }
                else enemies.Remove(e);
            }
               

            Random random = new Random();
            int newCoin = random.Next(1, 200);
            if (newCoin == 1)
            {
                int rndX = random.Next(0, Window.ClientBounds.Width - goldCoinSprite.Width);

                int rndY = random.Next(0, Window.ClientBounds.Height - goldCoinSprite.Height);

                goldCoins.Add(new GoldCoin(goldCoinSprite, rndX, rndY, gameTime));
            }

            foreach (GoldCoin gc in goldCoins.ToList())
            {
                if (gc.IsAlive)
                {
                    gc.Update(gameTime);

                    if (gc.CheckCollision(player))
                    {
                        goldCoins.Remove(gc);
                        player.Points++;
                    }
                }
                else
                    goldCoins.Remove(gc);
            }

                            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            foreach (Enemy e in enemies.ToList())
                e.Draw(spriteBatch);
            printText.Print("Antal Fiender : " +  enemies.Count, spriteBatch , 0, 0);
            foreach (GoldCoin gc in goldCoins)
                gc.Draw(spriteBatch);
            printText.Print("Points: " + player.Points, spriteBatch, 0, 20);
            spriteBatch.End();
            

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
