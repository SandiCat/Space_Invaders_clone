using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Sandi_s_Way;


namespace Space_Invaders_clone
{
    public class Invasion : GameObject
    {
        public Invasion()
            : base()
        {
        }
        public Invasion(Vector2 position)
            : base(position)
        {
        }

        public BaseInvader[,] Invaders = new BaseInvader[5, 9];
        public Vector2 Positon = new Vector2(64, 64);

        public void FillInvaders()
        {
            float yPosition = Positon.X;
            int row = Invaders.GetLength(1); //Gets the number of invaders in a row
            float spacing = 15.0f; //The space between the rows
            int invaderWidth = new InvaderType3().Sprite.GetRectangle().Width;
            int invaderHeight = new InvaderType3().Sprite.GetRectangle().Height;

            //Fill the first row with invaders type 1
            for(int i = 0; i < row; i++)
            {
                Invaders[0, i] = (BaseInvader)CreateAndReturnObject(typeof(InvaderType1), new Vector2(invaderWidth * i + Positon.X + 10 * i, yPosition)); 
            }
           
            //Fill the second and third row with invaders type 2
            yPosition += invaderHeight + spacing;
            for (int i = 0; i < row; i++)
            {
                Invaders[1, i] = (BaseInvader)CreateAndReturnObject(typeof(InvaderType2), new Vector2(invaderWidth * i + Positon.X + 10 * i, yPosition));
            }
            yPosition += invaderHeight + spacing;
            for (int i = 0; i < row; i++)
            {
                Invaders[2, i] = (BaseInvader)CreateAndReturnObject(typeof(InvaderType2), new Vector2(invaderWidth * i + Positon.X + 10 * i, yPosition));
            }

            //Fill the forth and fifth row with invaders type 3
            yPosition += invaderHeight + spacing;
            for (int i = 0; i < row; i++)
            {
                Invaders[3, i] = (BaseInvader)CreateAndReturnObject(typeof(InvaderType3), new Vector2(invaderWidth * i + Positon.X + 10 * i, yPosition));
            }
            yPosition += invaderHeight + spacing;
            for (int i = 0; i < row; i++)
            {
                Invaders[4, i] = (BaseInvader)CreateAndReturnObject(typeof(InvaderType3), new Vector2(invaderWidth * i + Positon.X + 10 * i, yPosition));
            }
        }

        public override void Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                FillInvaders();
            }
        }
    }
}
