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
    public static class StarMaker
    {
        //Boundaries for the randomness of size of stars
        private const float _topSize = 3;
        private const float _bottomSize = 0.2f;

        //Boundaries for the randomness of the number of stars
        private const int _topCount = 10;
        private const int _bottomCount = 4;

        public static void MakeSomeStars()
        {
            int screenWidth = GameInfo.RefDevice.Viewport.Width;
            int screenHeight = GameInfo.RefDevice.Viewport.Height;

            Random rand = ObjectManager.Rand;

            int howMuchStars = rand.Next(_bottomCount, _topCount);

            for (int i = 0; i < howMuchStars; i++)
            {
                Vector2 position = new Vector2(rand.Next(0, screenWidth), rand.Next(0, screenHeight));

                double range = (double)_topSize - (double)_bottomSize;
                double sample = rand.NextDouble();
                double scaled = (sample * range) + _bottomSize;
                float size = (float)scaled;

                ObjectManager.CreateAndReturn(typeof(Star), position).Sprite.Scale = size;

                //ObjectManager.Create(typeof(Star), position);
            }
        }
    }
}
