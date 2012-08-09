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
    public class Score : GameObject
    {
        public Score()
            : base()
        {
        }
        public Score(Vector2 position)
            : base(position)
        {
        }

        public static SpriteFont Font;
        public int Amount = 0;
        public Vector2 Position = new Vector2(10, 10);

        public override void Draw()
        {
            GameInfo.RefSpriteBatch.DrawString(Font, Amount.ToString(), Position, Color.Yellow);
        }
    }
}
