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
        public float ShootChance = 0.003f;
        private float spacing = 15.0f; //The space between the rows
        private DirectionMoving direction = DirectionMoving.Left;
        private int rows;
        private int columns;

        private int InvaderWidth;
        private int InvaderHeight;

        private int InvasionWidth;
        private int InvasionHeight;

        public void FillInvaders()  
        {
            float yPosition = Positon.Y;
            int row = columns; //Gets the number of invaders in a row

            //Fill the invaders:
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    if (i == 0)
                    {
                        Invaders[i, j] = (BaseInvader)CreateAndReturnObject
                        (typeof(InvaderType1), new Vector2(InvaderWidth * j + Positon.X + 10 * j, yPosition));
                    }
                    else if (i == 1 | i == 2)
                    {
                        Invaders[i, j] = (BaseInvader)CreateAndReturnObject
                        (typeof(InvaderType2), new Vector2(InvaderWidth * j + Positon.X + 10 * j, yPosition));
                    }
                    else if (i == 3 | i == 4)
                    {
                        Invaders[i, j] = (BaseInvader)CreateAndReturnObject
                        (typeof(InvaderType3), new Vector2(InvaderWidth * j + Positon.X + 10 * j, yPosition));
                    }
                }

                yPosition += InvaderHeight + spacing;
            }
        }
        private Vector2 GetTopLeft()
        {
            return Invaders[0, 0].Sprite.Position;
        }
        public void LevelUp() //Makes invasion faster and deadlier
        {
            ShootChance = ShootChance + (ShootChance * 0.5f); //Increase ShootChance by 50%
            Time = Time - (int)(Time * 0.1); //Decrease time by 10%
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

                rows = Invaders.GetLength(0);
                columns = Invaders.GetLength(1);
                FillInvaders();
            }
        }
        public override void Alarm(string name)
        {
            if (name == "move")
            {
                #region move
                //Get the rectangles for move checks:
                Rectangle invaders = new Rectangle((int)GetTopLeft().X, (int)GetTopLeft().Y, InvasionWidth, InvasionHeight);
                Rectangle screen = new Rectangle(0, 0, SpaceInvaders.Device.Viewport.Width, SpaceInvaders.Device.Viewport.Height);

                if (direction == DirectionMoving.Left)
                {
                    //Check if you can move to left (if there is space):
                    if (invaders.Left - BaseInvader.HowMuchToMove >= screen.Left + BaseInvader.HowMuchToMove)
                    {
                        //If so, move:
                        foreach (var invader in Invaders)
                        {
                            if (invader != null) invader.MoveLeft();
                        }
                    }
                    else
                    {
                        foreach (var invader in Invaders)
                        {
                            if (invader != null)  invader.MoveDown();
                            direction = DirectionMoving.Right;
                        }
                    }
                }
                else if (direction == DirectionMoving.Right)
                {
                    //Check if you can move to left (if there is space):
                    if (invaders.Right + BaseInvader.HowMuchToMove <= screen.Right + BaseInvader.HowMuchToMove)
                    {
                        //If so, move:
                        foreach (var invader in Invaders)
                        {
                            if (invader != null) invader.MoveRight();
                        }
                    }
                    else
                    {
                        foreach (var invader in Invaders)
                        {
                            if (invader != null) invader.MoveDown();
                            direction = DirectionMoving.Left;
                        }
                    }
                }
                #endregion

                Alarms["move"].Restart(Time);
            }
        }
        public override void Update()
        {
            #region shoot
            if (ObjectManager.Rand.NextDouble() < ShootChance)
            {
                //Get all the bottom invaders of all the columns
                List<BaseInvader> bottomRow = new List<BaseInvader>();
                
                for (int i = 0; i < columns; i++) //for each column
                {
                    for (int j = rows - 1; j >= 0; j--) //go from the bottom up, the first invader you find is the bottom one
                    {
                        if (Invaders[j, i] != null)
                        {
                            bottomRow.Add(Invaders[j, i]);
                            break;
                        }
                    }
                }

                int shooterIndex = ObjectManager.Rand.Next(bottomRow.Count);
                bottomRow[shooterIndex].Shoot();
            }
            #endregion

            #region Remove destroyed invaders

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (Invaders[i, j] != null)
                    {
                        if (Invaders[i, j].IsDestroyed())
                        {
                            Invaders[i, j] = null;
                        }
                    }
                }
            }

            #endregion
        }

        //Test:
        public override void KeyPressed(List<Keys> keys)
        {
            if (keys.Contains(Keys.B))
            {
                LevelUp();
            }
        }
    }
}
