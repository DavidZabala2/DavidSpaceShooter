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
    //Menu annvänds för at skapa en meny, lägga till menyval i menyn
    //samt att ta emot tangenttryckningar för olika menyval och att rita ut spel skärmen
    class Menu
    {
        List<MenuItem> menu;// rn generisk lista av MenuItems
        int selected = 0;// första valet i listan är valt

        float currentHeight = 0; //detta används för att rita ut menyItems på olika höjder
        double lastChange = 0;//används för att "pausa" tangenttryckningar så att de inte ska gå för fort att bläddra mellan navigationbaren
                              
        int defaultMenuState;//den state som representerar själva menyn:

        //Menu (), konstruktorn som skapar listan med MenuItem:s
        public Menu(int defaultMenuState)
        {
            menu = new List<MenuItem>();
            this.defaultMenuState = defaultMenuState;
        }
        //AddItem(), lägger till ett menyvali listan
        public void AddItem(Texture2D itemTexture, int state)
        {
            //Sätt höjd på item:
            float X = 0;
            float Y = 0 + currentHeight;

            //ändra currenthight efter detta items höjd + 20 pixlar för
            //lite extra mellanrum
            currentHeight += itemTexture.Height + 20;

            //Skapa ett temporträrt objekt och lägg det i listan:
            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), state);
            menu.Add(temp);
        }
        //Update(), kollar om använderen har tryckt någon tangent. Antingen kan piltangenter 
        //används för att välja en viss MenuItem(utan att gåin just in i de valet) eller så kan 
        //ENTER användas för att gå in i den valda MenuItem
        public int Update(GameTime gameTime)
        {
            //Las in tangenttryckningar
            KeyboardState keyboardState = Keyboard.GetState();

            //Byte nellan olika menyval. Först måste vi dock kontrollera
            //så att användaren inte precis nyligen bytte menyval. Vi vill
            //Därför pausar vi i 130 millisekunder:
            if (lastChange + 130 < gameTime.TotalGameTime.TotalMilliseconds)
            {
                //gå ett steg ned i menyn
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    selected++;
                    // om vi har gått utanför de möjliga valen, så vill vi att det första menyvaletska väljas:
                    if (selected > menu.Count - 1)
                        selected = 0;

                }
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    selected--;
                    //Om vi har gått utanför de möjliga valen alltså negativt siffror så vill vi att det sista & menyvalet ska väljas
                    if (selected < 0)
                        selected = menu.Count - 1;
                }
                //Ställ lastchange till exakt detta ögonblick
                lastChange = gameTime.TotalGameTime.TotalMilliseconds;
            }
            //Välj ett menyval med ENTER:
            if (keyboardState.IsKeyDown(Keys.Enter))
                return menu[selected].State;

            // om inget menyval har valts, så stannar vi kvar i menyn:
            return defaultMenuState;
        }
        //Draw(), ska rita ut menyn på skärmen
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menu.Count; i++)
            {
                //om vi har ett aktivt menyval ritar vi ut det med en speciell färgtoning:
                if (i == selected)
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.RosyBrown);
                else //annars ingen färgtoning alls.
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.White);
            }
        }
    }

    // MenuItem, container - klass för ett menyval
    class MenuItem
    {
        Texture2D texture; //bilden för menyvalet
        Vector2 position;//positionen för menyvalet
        int currentState;//menyvalets state

        //MenuItem(), konstruktorn som sätter värden för de olika menyvalen
        public MenuItem(Texture2D texture, Vector2 position, int currentState)
        {
            this.texture = texture;
            this.position = position;
            this.currentState = currentState;

        }
        //(Get-)egenskaper för MenuItem
        public Texture2D Texture { get { return texture; } }
        public Vector2 Position { get { return position; } }
        public int State { get { return currentState; } }
    }
}
