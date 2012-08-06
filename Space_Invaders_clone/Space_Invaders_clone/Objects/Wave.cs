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

    public class Wave : GameObject
    {
        public Wave()
            : base()
        {
        }
        public Wave(Vector2 position)
            : base(position)
        {
        }

        public BaseInvader[,] Invaders = new BaseInvader[5, 9];
        public Vector2 Positon;
        public int Time = 50;
        public float ShootChance = 0.003f;
        private float spacing = 15.0f; //The space between the rows
        private DirectionMoving direction = DirectionMoving.Right;
        private int rows;
        private int columns;
        private int moveSoundCounter = 4; //this is used so i can play the move sound with a different pitch each fourth time

        private int InvaderWidth;
        private int InvaderHeight;

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
        public void LevelUp() //Makes the wawe faster and deadlier
        {
            ShootChance += ShootChance * 0.5f; //Increase ShootChance by 50%
            Time -= (int)(Time * 0.1); //Decrease time by 10%
        }
        public bool IsEmpty()
        {
            bool isEmpty = true;
            foreach (var invader in Invaders)
            {
                if (invader != null)
                {
                    isEmpty = false;
                    break;
                }
                
            }

            return isEmpty;
        }
        public Rectangle GetInvadersRectangle()
        {
            int furthestLeft = 0;
            int furthestRight = 0;
            int furthestUp = 0;
            int furthestDown = 0;

            #region Find furthest left
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (Invaders[j, i] != null)
                    {
                        furthestLeft = Invaders[j, i].Sprite.GetRectangle().Left;

                        goto endSearchLeft; //im only using goto to break out of two loops. im sorry.
                    }
                }
            }
            endSearchLeft:
            #endregion

            #region Find furthest right
            for (int i = columns - 1; i >= 0; i--)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (Invaders[j, i] != null)
                    {
                        furthestRight = Invaders[j, i].Sprite.GetRectangle().Right;

                        goto endSearchRight; //im only using goto to break out of two loops. im sorry.
                    }
                }
            }
            endSearchRight:
            #endregion

            #region Find furthest up
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (Invaders[i, j] != null)
                    {
                        furthestUp = Invaders[i, j].Sprite.GetRectangle().Top;

                        goto endSearchUp; //im only using goto to break out of two loops. im sorry.
                    }
                }
            }
            endSearchUp:
            #endregion

            #region Find furthest down
            for (int i = rows - 1; i >= 0; i--)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (Invaders[i, j] != null)
                    {
                        furthestDown = Invaders[i, j].Sprite.GetRectangle().Bottom;

                        goto endSearchDown; //im only using goto to break out of two loops. im sorry.
                    }
                }
            }
            endSearchDown:
            #endregion

            Vector2 topLeft = new Vector2(furthestLeft, furthestUp);
            int width = furthestRight - furthestLeft;
            int height = furthestDown - furthestUp;

            return new Rectangle((int)topLeft.X, (int)topLeft.Y, width, height);
        }
        private void PlayMoveSound()
        {
            //Update move sound counter:
            if (moveSoundCounter > 0) moveSoundCounter--;
            else moveSoundCounter = 3;

            //Play the sound acordingly:
            BaseInvader.MoveSound.Play(1.0f, 0.1f * moveSoundCounter, 0.0f);
        }

        public override void Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                Positon = Sprite.Position;
                
                Alarms.Add("move", new Alarm(Time));

                InvaderWidth = new InvaderType1().Sprite.GetRectangle().Width;
                InvaderHeight = new InvaderType1().Sprite.GetRectangle().Height;

                rows = Invaders.GetLength(0);
                columns = Invaders.GetLength(1);
                FillInvaders();
            }
        }
        public override void Alarm(string name)
        {
            if (name == "move")
            {
                #region Move
                //Get the rectangles for move checks:
                Rectangle invaders = GetInvadersRectangle();
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
                        PlayMoveSound();
                    }
                    else
                    {
                        foreach (var invader in Invaders)
                        {
                            if (invader != null)  invader.MoveDown();
                        }

                        direction = DirectionMoving.Right;
                        PlayMoveSound();
                        Time -= (int)(Time * 0.1); //Decrease time by 10%
                    }
                }

                else if (direction == DirectionMoving.Right)
                {
                    //Check if you can move to left (if there is space):
                    if (invaders.Right + BaseInvader.HowMuchToMove <= screen.Right - BaseInvader.HowMuchToMove)
                    {
                        //If so, move:
                        foreach (var invader in Invaders)
                        {
                            if (invader != null) invader.MoveRight();
                        }

                        PlayMoveSound();
                    }
                    else
                    {
                        foreach (var invader in Invaders)
                        {
                            if (invader != null) invader.MoveDown();
                        }

                        direction = DirectionMoving.Left;
                        PlayMoveSound();
                        Time -= (int)(Time * 0.1); //Decrease time by 10%
                    }
                }
                #endregion

                Alarms["move"].Restart(Time);
            }
        }
        public override void Update()
        {
            #region Shoot
            if (!IsEmpty() && ObjectManager.Rand.NextDouble() < ShootChance)
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
        public override void Destroy(GameObject destroyedObject)
        {
            if (destroyedObject == this)
            {
                foreach (var invader in Invaders)
                {
                    DestroyObject(invader);
                }
            }
        }

        //Test:
        public override void KeyPressed(List<Keys> keys)
        {
            if (keys.Contains(Keys.B))
            {
                LevelUp();
            }
        }
        //public override void Draw()
        //{
        //    Rectangle rectangle = GetInvadersRectangle();

        //    Texture2D RectangleTexture = TextureContainer.ColoredRectangle(Color.PeachPuff, rectangle.Width, rectangle.Height);

        //    GameInfo.RefSpriteBatch.Draw(RectangleTexture, rectangle, Color.White);
        //} //for debugging
    }
}
