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
    public abstract class BaseInvader : GameObject
    {
        //Moving variables
        public static int HowMuchToMove = 8;
        public static int HowMuchDown = 50;
        public static int BulletSpeed = 3;

        public BaseInvader()
            : base()
        {
        }
        public BaseInvader(Vector2 position)
            : base(position)
        {
        }

        public void MoveLeft()
        {
            StepAngle(Directions.Left, HowMuchToMove);
        }
        public void MoveRight()
        {
            StepAngle(Directions.Right, HowMuchToMove);
        }
        public void MoveDown()
        {
            StepAngle(Directions.Down, HowMuchDown);
        }
        public void Shoot()
        {
            Vector2 belowInvader = Sprite.Position + new Vector2(53 / 2 - 5, 32);
            CreateMovingObject(typeof(EnemyBullet), belowInvader, Directions.Down, BulletSpeed);
        }

        public override void Collision(List<GameObject> collisions)
        {
            foreach (var obj in collisions)
            {
                if (obj.GetType() == typeof(PlayerBullet))
                {
                    DestroyObject(this);
                }
            }
        }
    }
}
