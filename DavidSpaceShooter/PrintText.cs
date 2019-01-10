using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DavidSpaceShooter

{ //PrintText, klassen för att skriva ut text på skärmen
    class PrintText
    {
        SpriteFont font;

        //PrintText(), konstruktorn som tar ett SpriteFont-objekt dvs en font som som har laddats in via content.
        public PrintText(SpriteFont font)
        {
            this.font = font;
        }
        //Print(), skriver ut texten på skärmen
        public void Print(string text, SpriteBatch spriteBatch, int X, int Y)
        {
            spriteBatch.DrawString(font, text, new Vector2(X, Y), Color.White);
        }
    }
}
