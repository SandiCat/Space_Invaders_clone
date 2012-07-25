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
    public class Player : GameObject
    {
        public Player()
            : base()
        {
        }
        public Player(Vector2 position)
            : base(position)
        {
        }

        int speed = 5;
        int bulletSpeed = 5;
        bool canShoot = true;
        int canShootTime = 60;

        public override void Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                Alarms.Add("reset canShoot", new Alarm());
            }
        }

        public override void KeyDown(List<Keys> keys)
        {
            if (keys.Contains(Keys.Space) && canShoot)
            {
                Vector2 abovePlayer = Sprite.Position + new Vector2(53 / 2 - 5, - 32);
                CreateMovingObject(typeof(PlayerBullet), abovePlayer, Directions.Up, bulletSpeed);
                
                canShoot = false;
                Alarms["reset canShoot"].Restart(canShootTime);
            }

            if (keys.Contains(Keys.Left))
            {
                StepAngle(Directions.Left, speed);
            }

            if (keys.Contains(Keys.Right))
            {
                StepAngle(Directions.Right, speed);
            }
        }

        public override void Alarm(string name)
        {
            if (name == "reset canShoot")
            {
                canShoot = true;
            }
        }

        public override void Collision(List<GameObject> collisions)
        {
            foreach (var obj in collisions)
            {
                if (obj.GetType() == typeof(EnemyBullet))
                {
                    DestroyObject(this);
                }
            }
        }
    }
}
