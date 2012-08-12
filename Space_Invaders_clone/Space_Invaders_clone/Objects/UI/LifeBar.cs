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

        private Texture2D _greenPellet;
        private Texture2D _redPellet;
        private const int _beginingHitPoints = 10;
        public int HitPoints = _beginingHitPoints;
        public static SoundEffect PlayerHit;

        public void TakeDamage(int howMuch)
        {
            HitPoints -= howMuch;

            float randomPitch = (float)ObjectManager.Rand.NextDouble();
            PlayerHit.Play(1.0f, randomPitch, 0.0f);
        }

        public override void  Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                _greenPellet = TextureContainer.ColoredRectangle(Color.Green, 10, 18);
                _redPellet = TextureContainer.ColoredRectangle(Color.Red, 10, 18);
            }
        }
        public override void Draw()
        {
            float x = Sprite.Position.X;
            float y = Sprite.Position.Y;

            for (int i = 0; i <= _beginingHitPoints; i++)
            {
                if (i <= HitPoints)
                    GameInfo.RefSpriteBatch.Draw(_greenPellet, new Vector2(x, y), Color.White);
                else
                    GameInfo.RefSpriteBatch.Draw(_redPellet, new Vector2(x, y), Color.White);

                x += 10;
            }
        }
    }
}
