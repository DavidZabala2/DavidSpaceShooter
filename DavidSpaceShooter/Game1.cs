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
    /// 
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        HighScore highscore;
        SpriteFont myFont;
       

        enum State { PrintHighScore, EnterHighScore };
        State currentState;
        
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

            GameElements.currentState = GameElements.State.Menu;
            GameElements.Initialize();
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
            myFont = Content.Load<SpriteFont>("myFont");
            highscore = new HighScore(5, myFont);
            /* highscore.LoadFromFile("highscore.txt"); */
         
            GameElements.LoadContent(Content, Window);
            
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
               this.Exit();

           

            switch (GameElements.currentState)
            {
                case GameElements.State.Run:
                    GameElements.currentState = GameElements.RunUpdate(Content, Window, gameTime);
                    break;
                case GameElements.State.AddHS:
                    GameElements.currentState = GameElements.AddHSUpdate(gameTime, Window, Content);
                    break;
                case GameElements.State.HighScore:
                    GameElements.currentState = GameElements.HighScoreUpdate(Window);
                    break;
                case GameElements.State.Quit:
                    this.Exit();
                    break;
                default:
                    GameElements.currentState = GameElements.MenuUpdate(gameTime);
                    break;

            }
                    base.Update(gameTime);
        }

      
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            switch (GameElements.currentState) //DE OLIKA LÄGEN I SPELET, Ritas ut - DAVID
            {
                case GameElements.State.Run:
                    GameElements.RunDraw(spriteBatch);
                   
                    break;
                case GameElements.State.AddHS:
                    GameElements.AddHSDraw(spriteBatch);
                    break;

                case GameElements.State.HighScore:
                    GameElements.HighScoreDraw(spriteBatch);
                    switch (currentState)
                    {

                        case State.EnterHighScore:
                            highscore.EnterDraw(spriteBatch);
                            break;
                    }
                    break;
                case GameElements.State.Quit:
                    this.Exit();
                    break;
                default: GameElements.MenuDraw(spriteBatch);
                    break;
            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
