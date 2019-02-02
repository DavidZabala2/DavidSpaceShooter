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
    //TL-190202 Se kommentar i ert klassdiagram.

    //Player2, klass för att skapa ett spelarobjekt. Klassen ska
    //hantera spelerans rörelse(sprite), de ska däremot ta in
    //tangentintryckningar för att förändra spelarens direktion
    class Player2 : PhysicalObject
    {

        List<Bullet2> bulletss; //Här skapas en generisk lista för att man ska kunna skjuta alla skott

        Texture2D bulletTexture; // är skotters bild
        double timeSinceLastBullet = 0; //I millisekunder
        int points = 0;// Anger antalet poäng som man har från början


        public int Points { get { return points; } set { points = value; } }
        public List<Bullet2> Bulletss { get { return bulletss; } }

        //Detta är Player2() Konstruktorn för att skapa spelar-objekt 
        public Player2(Texture2D texture, float X, float Y, float speedX, float speedY, Texture2D bulletTexture)
            : base(texture, X, Y, speedX, speedY)
        {
            bulletss = new List<Bullet2>();
            this.bulletTexture = bulletTexture;
        }
        //Reset(), återställer spelaren för ett nytt spel
        public void Reset(float X, float Y, float speedX, float speedY)
        {
            //Detta återställer spelarens po´sition och hastighet
            vector.X = X;
            vector.Y = Y;
            speed.X = speedX;
            speed.Y = speedY;
            //Återställer alla skott
            bulletss.Clear();
            timeSinceLastBullet = 0;
            //Återställer spelarens poäng på nytt:
            points = 0;
            //Gör så att spelaren lever igen
            isAlive = true;
        }
        //Update() Flyttar på spelaren
        public void Update(GameWindow window, GameTime gameTime)
        {
            //Läser in tangentintyckningar 
            KeyboardState keyboardState = Keyboard.GetState();

            //Detta ska hjälpa för att flytta på spelarens rörelse i spelet på alla sidor eftersom området i spelet begränsas
            //och så att man är inte påväg mot kanten.
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
            //Kontrollerar ifall spelaren har åkt på kanten,
            //om det har det, så återställa dess position
            //har det åkt ut till vänster.
            if (vector.X < 0)
                vector.X = 0;
            //Här så åker spelaren till höger
            if (vector.X > window.ClientBounds.Width - texture.Width)
            {
                vector.X = window.ClientBounds.Width - texture.Width;
            }
            //Här så åker spelaren till vänster
            if (vector.Y < 0)
                vector.Y = 0;
            //Här så åker spelaren med sin riktning nedåt.
            if (vector.Y > window.ClientBounds.Height - texture.Height)
            {
                vector.Y = window.ClientBounds.Height - texture.Height;
            }
            //Här så gäller det för player 2 om man vill avfyra flera skott så trycker man på F eller f
            if (keyboardState.IsKeyDown(Keys.F))
            {
                //Vi kontrollerar om spelaren får avfyra sina skott
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
                {
                    //bullet skapas här dvs skotten:
                    Bullet2 temps = new Bullet2(bulletTexture, vector.X + texture.Width / 2, vector.Y);
                    bulletss.Add(temps);// deta lägger till skott från den genersika listan så att de genereras och körs om som en loop tills man väljer inte att trycka på shift
                    timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;//Variablet timeSinceLastBullet anger ögnonblicket som skottet ska forsätta avfyras per sekund eller inte.
                
            }
            }
            //Flyttar på alla skotts riktningar 
            foreach (Bullet2 bs in bulletss.ToList())
            {
                //Här så flyttas alla skott
                bs.Update();
                //De kontrollerar så att skotten inte ska vara "dött" dvs att inget avfyras
                if (!bs.IsAlive)
                    //Detta tar bort skottet ur listan
                    bulletss.Remove(bs);
            }
            //genom att trycka på escape knappen så återvänder man tillbaka till menyn
            if (keyboardState.IsKeyDown(Keys.Escape))
                isAlive = false;
        }


        //Draw(), ritar ut bilden på spelskärmen
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, Color.White);
            foreach (Bullet2 bs in bulletss)
                bs.Draw(spriteBatch);
        }
    }
    //Bullet2, en klass för att skapa skottet
    class Bullet2 : PhysicalObject
    {
        //Bullet() konstruktorn för att skapa ett skott-objekt
        public Bullet2(Texture2D texture, float X, float Y)
            : base(texture, X, Y, 0, 7f)
        {
        }
        //Update(), uppdaterar skotters position och tar bort de när de kommer i kollosion med fiende 
        //eller när ett skott åker utanförfönstret
        public void Update()
        {
            vector.Y -= speed.Y;
            if (vector.Y < 0)
                isAlive = false;
        }
    }
}



