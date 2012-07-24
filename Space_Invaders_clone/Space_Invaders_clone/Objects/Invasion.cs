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
    enum DirectionMoving
    {
        Left,
        Right
    }

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
        public Vector2 Positon;
        public int Time = 50;
        private float spacing = 15.0f; //The space between the rows
        private DirectionMoving direction = DirectionMoving.Left;

        private int InvaderWidth;
        private int InvaderHeight;

        private int InvasionWidth;
        private int InvasionHeight;

        public void FillInvaders()
        {
            float yPosition = Positon.Y;
            int row = Invaders.GetLength(1); //Gets the number of invaders in a row

            //Fill the first row with invaders type 1
            for(int i = 0; i < row; i++)
            {
                Invaders[0, i] = (BaseInvader)CreateAndReturnObject(typeof(InvaderType1), new Vector2(InvaderWidth * i + Positon.X + 10 * i, yPosition)); 
            }
           
            //Fill the second and third row with invaders type 2
            yPosition += InvaderHeight + spacing;
            for (int i = 0; i < row; i++)
            {
                Invaders[1, i] = (BaseInvader)CreateAndReturnObject(typeof(InvaderType2), new Vector2(InvaderWidth * i + Positon.X + 10 * i, yPosition));
            }
            yPosition += InvaderHeight + spacing;
            for (int i = 0; i < row; i++)
            {
                Invaders[2, i] = (BaseInvader)CreateAndReturnObject(typeof(InvaderType2), new Vector2(InvaderWidth * i + Positon.X + 10 * i, yPosition));
            }

            //Fill the forth and fifth row with invaders type 3
            yPosition += InvaderHeight + spacing;
            for (int i = 0; i < row; i++)
            {
                Invaders[3, i] = (BaseInvader)CreateAndReturnObject(typeof(InvaderType3), new Vector2(InvaderWidth * i + Positon.X + 10 * i, yPosition));
            }
            yPosition += InvaderHeight + spacing;
            for (int i = 0; i < row; i++)
            {
                Invaders[4, i] = (BaseInvader)CreateAndReturnObject(typeof(InvaderType3), new Vector2(InvaderWidth * i + Positon.X + 10 * i, yPosition));
            }
        }
        private Vector2 GetTopLeft()
        {
            return Invaders[0, 0].Sprite.Position;
        }

        public override void Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                Positon = Sprite.Position;
                Alarms.Add("move", new Alarm(Time));

                InvaderWidth = new InvaderType1().Sprite.GetRectangle().Width;
                InvaderHeight = new InvaderType1().Sprite.GetRectangle().Height;

                InvasionWidth = Invaders.GetLength(1) * InvaderWidth + (Invaders.GetLength(1) - 1) * (int)spacing;
                InvasionHeight = Invaders.GetLength(0) * InvaderHeight + (Invaders.GetLength(0) - 1) * (int)spacing;

                FillInvaders();


            }
        }

        public override void Alarm(string name)
        {
            if (name == "move")
            {
                //Get the rectangles for move checks:
                Rectangle invaders = new Rectangle((int)GetTopLeft().X, (int)GetTopLeft().Y, InvasionWidth, InvasionHeight);
                Rectangle screen = new Rectangle(0, 0, SpaceInvaders.Device.Viewport.Width, SpaceInvaders.Device.Viewport.Height);

                if (direction == DirectionMoving.Left)
                {
                    //Check if you can move to left (if there is space):
                    if (invaders.Left - BaseInvader.HowMuchToMove >= screen.Left + 16)
                    {
                        //If so, move:
                        foreach (var invader in Invaders)
                        {
                            invader.MoveLeft();
                        }
                    }
                    else
                    {
                        foreach (var invader in Invaders)
                        {
                            invader.MoveDown();
                            direction = DirectionMoving.Right;
                        }
                    }
                }
                else if (direction == DirectionMoving.Right)
                {
                    //Check if you can move to left (if there is space):
                    if (invaders.Right + BaseInvader.HowMuchToMove <= screen.Right + 16)
                    {
                        //If so, move:
                        foreach (var invader in Invaders)
                        {
                            invader.MoveRight();
                        }
                    }
                    else
                    {
                        foreach (var invader in Invaders)
                        {
                            invader.MoveDown();
                            direction = DirectionMoving.Left;
                        }
                    }
                }

                Alarms["move"].Restart(Time);
            }
        }
    }
}
