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
        private class CheckObject : GameObject //This is used to check if the player is below blocks
        {
            public CheckObject()
                : base()
            {
            }
            public CheckObject(Vector2 position)
                : base(position)
            {
            }

            public Player Player;

            public override void Create(GameObject createdObject)
            {
                if (createdObject == this)
                {
                    Visable = false;
                    Solid = false;

                    int blockSide = new Block().Sprite.Image.Height; //Since a block is square shaped it dosen't matter wich side you take
                    Sprite = new Sprite(TextureContainer.ColoredRectangle(Color.White, 3, 
                        blockSide * 3), new Vector2(SpaceInvaders.Device.Viewport.Width / 2, BlockMaker.Position.Y));
                        //Creates it where the player shoots bullets
                }
            }
            public override void Update()
            {
                Sprite.Position.X = Player.Sprite.Position.X + 25; //If the size of the player or the bullet are ever changed, this would need tweaking
                Sprite.Position.Y= BlockMaker.Position.Y;
            }

            public bool IsCollidingWithBlocks()
            {
                foreach (var obj in ObjectManager.Objects)
                {
                    if (obj.GetType() == typeof(Block) && IsColliding(obj))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public Player()
            : base()
        {
        }
        public Player(Vector2 position)
            : base(position)
        {
        }

        int speed = 5;
        int bulletSpeed = 10;
        bool canShoot = true;
        int canShootTime = 40;
        int shortCanShootTime = 10; //for when player is under blocks

        public static SoundEffect ShootSound;
        CheckObject checker;

        public override void Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                Alarms.Add("reset canShoot", new Alarm());
                
                checker = (CheckObject)CreateAndReturnObject(typeof(CheckObject), new Vector2(0, 0));
                checker.Player = this;
            }
        }
        public override void KeyDown(List<Keys> keys)
        {
            if (keys.Contains(Keys.Left))
            {
                StepAngle(Directions.Left, speed);
            }

            if (keys.Contains(Keys.Right))
            {
                StepAngle(Directions.Right, speed);
            }

            if (keys.Contains(Keys.Space) && canShoot)
            {
                Vector2 abovePlayer = Sprite.Position + new Vector2(53f / 2f - 2.5f, 0);
                CreateMovingObject(typeof(PlayerBullet), abovePlayer, Directions.Up, bulletSpeed);

                canShoot = false;
                if (!checker.IsCollidingWithBlocks()) Alarms["reset canShoot"].Restart(canShootTime);
                else Alarms["reset canShoot"].Restart(shortCanShootTime);

                ShootSound.Play();
            }

            //Prevent the ship from leaving the screen:
            Rectangle screen = new Rectangle(0, 0, SpaceInvaders.Device.Viewport.Width, SpaceInvaders.Device.Viewport.Height);
            Sprite.Position.X = MathHelper.Clamp(Sprite.Position.X, screen.Left, screen.Right - Sprite.Image.Width);
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
                    if (SpaceInvaders.RefLife.HitPoints != 0)
                    {
                        SpaceInvaders.RefLife.TakeDamage(1);
                        DestroyObject(obj);
                    }
                    else
                    {
                        ObjectManager.Clear();
                        int screenWidth = SpaceInvaders.Device.Viewport.Width;
                        int screenHeight = SpaceInvaders.Device.Viewport.Height;
                        CreateObject(typeof(GameOverSign), new Vector2(screenWidth / 2, screenHeight / 2));
                    }
                }
            }
        }
    }
}
