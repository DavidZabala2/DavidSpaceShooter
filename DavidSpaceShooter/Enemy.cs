using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidSpaceShooter
{
    //Enemy(basklass) för fiender 
    abstract class Enemy : PhysicalObject
    {
        //Enemy(), kunstruktorn för att skapa objektet.
        public Enemy(Texture2D texture, float X, float Y, float speedX, float speedY)
            : base(texture, X, Y, speedX, speedY)
        {
        }

        //Update(), abstrakt metod som måste implementeras i alla härledda fiender. 
        //Används för att uppdatterafiendernas position.
        public abstract void Update(GameWindow window);
    }
    //Mine, är elaka bots som rör sig fram och tillbaka i skärmen
    class Mine : Enemy
    {
        //Mine(), konstruktorn för att skapa objektet.
        public Mine(Texture2D texture, float X, float Y)
            : base(texture, X, Y, 0.9f, 0.9f)
        {
        }
        //Update(), uppdaterar fiendens position
        public override void Update(GameWindow window)
        {
            //Flytta på fienden:
            vector.X += speed.X;
            //Kontrollerar så den inte åker utanför fönstret på sidorna
            if (vector.X > window.ClientBounds.Width - texture.Width || vector.X < 0)
                speed.X *= -1;//Byter rikktning på fienden
           
            vector.Y += speed.Y;
            //Gör fienden inaktiv om de åker ut från skärmen 
            if (vector.Y > window.ClientBounds.Height)
                isAlive = false;
        }
    }
    //Tripod, en annan yterligare elakere bot som rör sig i full fart ralt framåt.
    class Tripod : Enemy
    {
        //Tripod(), konstruktorn för att skapa objektet
        public Tripod(Texture2D texture, float X, float Y)
            : base(texture, X, Y, 5f, 0.1f)
        {
        }
        //Update(), fiendens postition
        public override void Update(GameWindow window)
        {
            //Flytta på fienden:
            vector.X += speed.X;
            if (vector.X > window.ClientBounds.Width - texture.Width || vector.X < 0)
                speed.X *= -1;
            //Gör fienden inaktiv om de åker ut från skärmen 
            vector.Y += speed.Y;//Byter rikktning på fienden
            if (vector.Y > window.ClientBounds.Height)
                isAlive = false;
        }
    }
}
