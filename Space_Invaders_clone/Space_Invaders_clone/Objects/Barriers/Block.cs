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
    public class Block : GameObject
    {
        public Block()
            : base()
        {
        }
        public Block(Vector2 position)
            : base(position)
        {
        }

        private int _hitPoints = 3;

        public override void Collision(List<GameObject> collisions)
        {
            foreach (var obj in collisions)
            {
                if (obj.GetType() == typeof(PlayerBullet) | obj.GetType() == typeof(EnemyBullet))
                {
                    _hitPoints--;
                    DestroyObject(obj);
                }
            }
        }
        public override void Update()
        {
            if (_hitPoints == 0) DestroyObject(this);
        }
    }
}
