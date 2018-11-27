using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidSpaceShooter
{
    class Player2 : PhysicalObject
    {

        List<Bullet2> bulletss;

        Texture2D bulletTexture;
        double timeSinceLastBullet = 0;
        int points = 0;

        public int Points { get { return points; } set { points = value; } }
        public List<Bullet2> Bulletss { get { return bulletss; } }


        public Player2(Texture2D texture, float X, float Y, float speedX, float speedY, Texture2D bulletTexture)
            : base(texture, X, Y, speedX, speedY)
        {
            bulletss = new List<Bullet2>();
            this.bulletTexture = bulletTexture;
        }
        public void Reset(float X, float Y, float speedX, float speedY)
        {
            vector.X = X;
            vector.Y = Y;
            speed.X = speedX;
            speed.Y = speedY;

            bulletss.Clear();
            timeSinceLastBullet = 0;
            points = 0;
            isAlive = true;
        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();


            if (vector.X <= window.ClientBounds.Width - texture.Width && vector.X >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.D))
                    vector.X += speed.X;
                if (keyboardState.IsKeyDown(Keys.A))
                    vector.X -= speed.X;
            }


            if (vector.Y <= window.ClientBounds.Height - texture.Height && vector.Y >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.S))
                    vector.Y += speed.Y;
                if (keyboardState.IsKeyDown(Keys.W))
                    vector.Y -= speed.Y;
            }

            if (vector.X < 0)
                vector.X = 0;

            if (vector.X > window.ClientBounds.Width - texture.Width)
            {
                vector.X = window.ClientBounds.Width - texture.Width;
            }

            if (vector.Y < 0)
                vector.Y = 0;

            if (vector.Y > window.ClientBounds.Height - texture.Height)
            {
                vector.Y = window.ClientBounds.Height - texture.Height;
            }
            if (keyboardState.IsKeyDown(Keys.F))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
                {
                    Bullet2 temps = new Bullet2(bulletTexture, vector.X + texture.Width / 2, vector.Y);
                    bulletss.Add(temps);
                    timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            foreach (Bullet2 bs in bulletss.ToList())
            {
                bs.Update();
                if (!bs.IsAlive)
                    bulletss.Remove(bs);
            }
            if (keyboardState.IsKeyDown(Keys.Escape))
                isAlive = false;
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, Color.White);
            foreach (Bullet2 bs in bulletss)
                bs.Draw(spriteBatch);
        }
    }
    class Bullet2 : PhysicalObject
    {
        public Bullet2(Texture2D texture, float X, float Y)
            : base(texture, X, Y, 0, 7f)
        {
        }
        public void Update()
        {
            vector.Y -= speed.Y;
            if (vector.Y < 0)
                isAlive = false;
        }
    }
}



