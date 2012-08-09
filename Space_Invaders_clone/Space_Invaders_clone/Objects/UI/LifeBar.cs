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
    public class LifeBar : GameObject
    {
        public LifeBar()
           : base()
        {
        }
        public LifeBar(Vector2 position)
           : base(position)
        {
        }

        Texture2D greenPellet;
        Texture2D redPellet;
        const int beginingHitPoints = 10;
        public int HitPoints = beginingHitPoints;

        public override void  Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                greenPellet = TextureContainer.ColoredRectangle(Color.Green, 10, 25);
                redPellet = TextureContainer.ColoredRectangle(Color.Red, 10, 25);
            }
        }
        public override void Draw()
        {
            float x = Sprite.Position.X;
            float y = Sprite.Position.Y;

            for (int i = 0; i <= beginingHitPoints; i++)
            {
                if (i <= HitPoints)
                    GameInfo.RefSpriteBatch.Draw(greenPellet, new Vector2(x, y), Color.White);
                else
                    GameInfo.RefSpriteBatch.Draw(redPellet, new Vector2(x, y), Color.White);

                x += 10;
            }
        }
    }
}
