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

        const int Speed = 2;
        const int Points = 1000;

        public static SoundEffect ExplodeSound;
        public static Vector2 PositionToCreate = new Vector2(0, 50);

        public override void Update()
        {
            StepAngle(Directions.Right, Speed);
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
                    SpaceInvaders.RefScore.AddPoints(Points);
                }
            }
        }

        public static void MakeIt()
        {
            ObjectManager.Create(typeof(SpecialInvader), PositionToCreate);
        }
    }
}
