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
    public class SpecialInvader : GameObject
    {
        public SpecialInvader()
            : base()
        {
        }
        public SpecialInvader(Vector2 position)
            : base(position)
        {
        }

        private const int _speed = 2;
        private const int _points = 1000;

        public static SoundEffect ExplodeSound;
        private static Vector2 _positionToCreate = new Vector2(0, 50); //Used in the "make it" method

        public override void Update()
        {
            StepAngle(Directions.Right, _speed);
        }
        public override void OutsideOfWindow()
        {
            DestroyObject(this);
        }
        public override void Collision(List<GameObject> collisions)
        {
            foreach (var obj in collisions)
            {
                if (obj.GetType() == typeof(PlayerBullet))
                {
                    DestroyObject(this);

                    ExplodeSound.Play(0.5f, 0.2f, 0.0f);
                    SpaceInvaders.RefScore.AddPoints(_points);
                }
            }
        }

        public static void MakeIt() //Makes a new special invader at the fixed position
        {
            ObjectManager.Create(typeof(SpecialInvader), _positionToCreate);
        } 
    }
}
