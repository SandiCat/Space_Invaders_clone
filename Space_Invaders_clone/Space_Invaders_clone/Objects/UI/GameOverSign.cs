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
    public class GameOverSign : GameObject
    {
         public GameOverSign()
            : base()
        {
        }
        public GameOverSign(Vector2 position)
            : base(position)
        {
        }

        public override void Create(GameObject createdObject)
        {
            Solid = false;
            Sprite.Scale = 4f;
            Sprite.Origin = new Vector2(50, 25 / 2);
        }
    }
}
