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
    public enum DirectionMoving
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

        private BaseInvader[,] _invaders = new BaseInvader[5, 9]; //Holds references to invaders stored in Objectanager.Objects
        private Vector2 _positon; //A reference to sprite position
        
        private int _moveTime = 50; //How long it takes for the wave to move
        private int _shootTimeTop = 60 * 3; //Max amount of time it will take the wave to shoot
        private int _shootTimeBottom = 60; //Min amount of time it will take the wave to shoot

        private float _spacing = 15.0f; //The space between the rows
        private int _rows;
        private int _columns;

        private DirectionMoving direction = DirectionMoving.Right;

        private int _moveSoundCounter = 4; //this is used so i can play the move sound with a descending pitch

        private int _invaderWidth;
        private int _invaderHeight;

        public void FillInvaders()
        {
            float yPosition = _positon.Y;
            int row = _columns; //Gets the number of invaders in a row

            //Fill the invaders:
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    if (i == 0)
                    {
                        _invaders[i, j] = (BaseInvader)CreateAndReturnObject
                        (typeof(InvaderType1), new Vector2(_invaderWidth * j + _positon.X + 10 * j, yPosition));
                    }
                    else if (i == 1 | i == 2)
                    {
                        _invaders[i, j] = (BaseInvader)CreateAndReturnObject
                        (typeof(InvaderType2), new Vector2(_invaderWidth * j + _positon.X + 10 * j, yPosition));
                    }
                    else if (i == 3 | i == 4)
                    {
                        _invaders[i, j] = (BaseInvader)CreateAndReturnObject
                        (typeof(InvaderType3), new Vector2(_invaderWidth * j + _positon.X + 10 * j, yPosition));
                    }
                }

                yPosition += _invaderHeight + _spacing;
            }
        }
        public void LevelUp() //Makes the wawe faster and deadlier
        {
            _shootTimeTop -= (int)(_shootTimeTop * 0.1); //Decrease by 10%
            _shootTimeBottom -= (int)(_shootTimeBottom * 0.1); //Decrease by 10%
            _moveTime -= (int)(_moveTime * 0.1); //Decrease by 10%
        }
        public bool IsEmpty()
        {
            bool isEmpty = true;
            foreach (var invader in _invaders)
            {
                if (invader != null)
                {
                    isEmpty = false;
                    break;
                }
                
            }

            return isEmpty;
        } //Returns true if all invaders are destroyed
        public Rectangle GetInvadersRectangle()
        {
            int furthestLeft = 0;
            int furthestRight = 0;
            int furthestUp = 0;
            int furthestDown = 0;

            #region Find furthest left
            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    if (_invaders[j, i] != null)
                    {
                        furthestLeft = _invaders[j, i].Sprite.GetRectangle().Left;

                        goto endSearchLeft; //im only using goto to break out of two loops. im sorry.
                    }
                }
            }
            endSearchLeft:
            #endregion

            #region Find furthest right
            for (int i = _columns - 1; i >= 0; i--)
            {
                for (int j = 0; j < _rows; j++)
                {
                    if (_invaders[j, i] != null)
                    {
                        furthestRight = _invaders[j, i].Sprite.GetRectangle().Right;

                        goto endSearchRight; //im only using goto to break out of two loops. im sorry.
                    }
                }
            }
            endSearchRight:
            #endregion

            #region Find furthest up
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_invaders[i, j] != null)
                    {
                        furthestUp = _invaders[i, j].Sprite.GetRectangle().Top;

                        goto endSearchUp; //im only using goto to break out of two loops. im sorry.
                    }
                }
            }
            endSearchUp:
            #endregion

            #region Find furthest down
            for (int i = _rows - 1; i >= 0; i--)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_invaders[i, j] != null)
                    {
                        furthestDown = _invaders[i, j].Sprite.GetRectangle().Bottom;

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
            if (_moveSoundCounter > 0) _moveSoundCounter--;
            else _moveSoundCounter = 3;

            //Play the sound acordingly:
            BaseInvader.MoveSound.Play(1.0f, 0.1f * _moveSoundCounter, 0.0f);
        }

        public override void Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                _positon = Sprite.Position;
                
                Alarms.Add("move", new Alarm(_moveTime));
                Alarms.Add("shoot", new Alarm(ObjectManager.Rand.Next(_shootTimeBottom, _shootTimeTop)));

                _invaderWidth = new InvaderType1().Sprite.GetRectangle().Width;
                _invaderHeight = new InvaderType1().Sprite.GetRectangle().Height;

                _rows = _invaders.GetLength(0);
                _columns = _invaders.GetLength(1);
                FillInvaders();
            }
        }
        public override void Alarm(string name) //Moves and shoots
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
                        foreach (var invader in _invaders)
                        {
                            if (invader != null) invader.Move(direction);
                        }
                        PlayMoveSound();
                    }
                    else
                    {
                        foreach (var invader in _invaders)
                        {
                            if (invader != null)  invader.MoveDown();
                        }

                        direction = DirectionMoving.Right;
                        PlayMoveSound();
                        _moveTime -= (int)(_moveTime * 0.1); //Decrease time by 10%
                    }
                }
                else if (direction == DirectionMoving.Right)
                {
                    //Check if you can move to left (if there is space):
                    if (invaders.Right + BaseInvader.HowMuchToMove <= screen.Right - BaseInvader.HowMuchToMove)
                    {
                        //If so, move:
                        foreach (var invader in _invaders)
                        {
                            if (invader != null) invader.Move(direction); 
                        }

                        PlayMoveSound();
                    }
                    else
                    {
                        foreach (var invader in _invaders)
                        {
                            if (invader != null) invader.MoveDown();
                        }

                        direction = DirectionMoving.Left;
                        PlayMoveSound();
                        _moveTime -= (int)(_moveTime * 0.1); //Decrease time by 10%
                    }
                }

                Alarms["move"].Restart(_moveTime);
                #endregion
            }
            
            if (name == "shoot")
            {
                #region Shoot
                if (!IsEmpty())
                {
                    //Get all the bottom invaders of all the columns
                    List<BaseInvader> bottomRow = new List<BaseInvader>();

                    for (int i = 0; i < _columns; i++) //for each column
                    {
                        for (int j = _rows - 1; j >= 0; j--) //go from the bottom up, the first invader you find is the bottom one
                        {
                            if (_invaders[j, i] != null)
                            {
                                bottomRow.Add(_invaders[j, i]);
                                break;
                            }
                        }
                    }

                    int shooterIndex = ObjectManager.Rand.Next(bottomRow.Count);
                    bottomRow[shooterIndex].Shoot();
                }

                Alarms["shoot"].Restart(ObjectManager.Rand.Next(_shootTimeBottom, _shootTimeTop));
                #endregion
            }
        }
        public override void Update() //Remove usless bullets
        {
            #region Remove destroyed invaders

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_invaders[i, j] != null)
                    {
                        if (_invaders[i, j].IsDestroyed())
                        {
                            _invaders[i, j] = null;
                        }
                    }
                }
            }

            #endregion
        }
        public override void Destroy(GameObject destroyedObject) //Destroy all invaders when destroyed
        {
            if (destroyedObject == this)
            {
                foreach (var invader in _invaders)
                {
                    DestroyObject(invader);
                }
            }
        }
    }
}
