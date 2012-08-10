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


        public Wave Current;
        public Vector2 MainPosition; //Where waves start
        int waveCounter = 1;
        int gameOverLine = 370;

        public override void Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                MainPosition = Sprite.Position;
                Current = (Wave)CreateAndReturnObject(typeof(Wave), MainPosition);
            }
        }
        public override void Update()
        {
            //Check if the wawe is destroyed
            if (Current.IsEmpty())
            {
                //Spawn new wawe
                DestroyObject(Current);
                Current = (Wave)CreateAndReturnObject(typeof(Wave), MainPosition);

                //Reset barrier blocks
                foreach (var obj in ObjectManager.Objects)
                {
                    if (obj.GetType() == typeof(Block))
                    {
                        DestroyObject(obj);
                    }
                }
                BlockMaker.MakeANewSetOfBlocks();

                for (int i = 0; i < waveCounter; i++)
                {
                    Current.LevelUp();
                }
            }

            //Check for a game over
            if (Current.GetInvadersRectangle().Bottom >= gameOverLine)
            {
                ObjectManager.Clear();
                int screenWidth = SpaceInvaders.Device.Viewport.Width;
                int screenHeight = SpaceInvaders.Device.Viewport.Height;
                CreateObject(typeof(GameOverSign), new Vector2(screenWidth / 2, screenHeight / 2));
            }
        }
    }
}
