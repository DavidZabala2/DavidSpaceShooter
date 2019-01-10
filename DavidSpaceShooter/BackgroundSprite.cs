using System;
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
    //BackgroundSprite, behållerklass för en bakgrundsbild. Denna typ av bakgrundsbild är
    //en del vektorer med flera BackgroundSprite-objekt.
    class BackgroundSprite : GameObject
    {
        //BakgroundSprite(), konstruktor för att skapa BackgroundSprite-objekt.
        public BackgroundSprite(Texture2D texture, float X, float Y) 
            : base (texture,X , Y)
        {

        }

        //Update(), ändrar positionen för BackgroundSprite-objekt. Flyttar det längst upp ifall 
        //det har gåt ut i nedkanten av skärmen
        public void Update(GameWindow window, int nrBackgroundsY)
        {
            vector.Y += 2f;
            if (vector.Y > window.ClientBounds.Height)
            {
                vector.Y = vector.Y - nrBackgroundsY * texture.Height;
            }
        }
    }
    //Bakground, klass för att rita ut en 2d-vektor med bakgrundsbilder.
    class Background
    {
        BackgroundSprite[,] background;
        int nrBackgroundsX, nrBackgroundsY;

        //Background(), konstruktorn som skapar alla BackgroundSprite-objekt
        // i en tvådimensionell

        public Background(Texture2D texture, GameWindow window)
        {
            //Hur många bilder ska vi ha på bredden?
            double tmpX = (double)window.ClientBounds.Width / texture.Width;
            //Avrunda uppåt med Math.Ceiling(tmpX);
            nrBackgroundsX = (int)Math.Ceiling(tmpX);
            //Hur många blder ska vi ha på höjden?
            double tmpY = (double)window.ClientBounds.Height / texture.Height;
            //Avrunda uppåt med Math.Ceiling och vi lägger till en extra     
            nrBackgroundsY = (int)Math.Ceiling(tmpY) + 1;
            //sätt storlek på vektorn
            background = new BackgroundSprite[nrBackgroundsX, nrBackgroundsY];

            //Fyll på vektorn ned BackgroundSprite-objekt:
            for (int i = 0; i < nrBackgroundsX; i++)
            {
                for (int j = 0; j < nrBackgroundsY; j++)
                {
                    //Gör att den först hamnar ovanför skärmen:
                    int posX = i * texture.Width;
                    int posY = j * texture.Height - texture.Height;
                    background[i, j] = new BackgroundSprite(texture, posX, posY);
                }
            }
        }
        //Update(), uppdaterar alla positioner för alla samtliga BackgroundSprite-objekt.
        public void Update(GameWindow window)
        {

            for (int i = 0; i < nrBackgroundsX; i++)
                for (int j = 0; j < nrBackgroundsY; j++)
                    background[i, j].Update(window, nrBackgroundsY);

        }
        //Draw(), ritar ut samtliga BackgroundSprite-objekt.
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < nrBackgroundsX; i++)
                for (int j = 0; j < nrBackgroundsY; j++)
                    background[i, j].Draw(spriteBatch);
        }
    }
}
