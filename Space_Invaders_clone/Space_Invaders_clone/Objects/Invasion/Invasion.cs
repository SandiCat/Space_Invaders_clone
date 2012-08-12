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
    //This object spawns a wave, and when its destroyed it makes a new one, and so on.
    //It also makes them gradually harder and it checks for game over
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


        private Wave _current; //The wave that currently on the screen
        private Vector2 _mainPosition; //Where waves start
        private int _waveCounter = 1;
        private int _gameOverLine = 370; //The x position that the bottom of the wave needs to pass to kill the player

        public override void Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                _mainPosition = Sprite.Position;
                _current = (Wave)CreateAndReturnObject(typeof(Wave), _mainPosition);
            }
        }
        public override void Update()
        {
            //Check if the wave is destroyed:
            if (_current.IsEmpty())
            {
                //Spawn new wave
                DestroyObject(_current);
                _current = (Wave)CreateAndReturnObject(typeof(Wave), _mainPosition);

                //Reset barrier blocks
                foreach (var obj in ObjectManager.Objects.Where(obj => obj.GetType() == typeof(Block))) //Destroy old ones
                {
                    DestroyObject(obj);
                }
                BlockMaker.MakeANewSetOfBlocks();

                //Level up the invasion
                for (int i = 0; i < _waveCounter; i++)
                {
                    _current.LevelUp();
                }
            }

            //Check for a game over:
            if (_current.GetInvadersRectangle().Bottom >= _gameOverLine)
            {
                //If game is indeed over than destroy everything and make a game over sign at the middle of the screen
                ObjectManager.Clear();
                int screenWidth = SpaceInvaders.Device.Viewport.Width;
                int screenHeight = SpaceInvaders.Device.Viewport.Height;
                CreateObject(typeof(GameOverSign), new Vector2(screenWidth / 2, screenHeight / 2));
            }
        }
    }
}
