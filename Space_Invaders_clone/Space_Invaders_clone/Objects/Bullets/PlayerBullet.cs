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
    public class PlayerBullet : GameObject
    {
        public PlayerBullet()
            : base()
        {
        }
        public PlayerBullet(Vector2 position)
            : base(position)
        {
        }

        public override void Collision(List<GameObject> collisions)
        {
            foreach (var obj in collisions)
            {
                if (typeof(BaseInvader).IsAssignableFrom(obj.GetType()))
                {
                    DestroyObject(this);
                }
            }
        }

        public override void IntersectBoundary()
        {
            base.IntersectBoundary();
        }
    }
}
