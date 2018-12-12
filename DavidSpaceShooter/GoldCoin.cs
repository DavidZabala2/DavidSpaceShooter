using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DavidSpaceShooter
{
    class GoldCoin : PhysicalObject
    {
        

        double timeToDie; //Hur länge lever myntet kvar

        //Konstruktor för att skapa objektet
        public GoldCoin(Texture2D texture, float X, float Y, GameTime gameTime)
            : base(texture, X, Y, 0, 2f)
        {
            timeToDie = gameTime.TotalGameTime.TotalMilliseconds + 5000;
        }
        public void Update(GameTime gameTime)
        {
            //Ta bort myntet om det är för gammalt.
            if (timeToDie < gameTime.TotalGameTime.TotalMilliseconds)
                isAlive = false;
        }
    }
}

