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
    public class Invasion : GameObject
    {
        public Invasion()
            : base()
        {
        }
        public Invasion(Vector2 position)
            : base(position)
        {
        }


        public Wawe Current;
        public Vector2 MainPosition; //Where wawes start

        public override void Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                MainPosition = Sprite.Position;
                Current = (Wawe)CreateAndReturnObject(typeof(Wawe), MainPosition);
            }
        }
        public override void Update()
        {
            if (Current.IsEmpty())
            {
                DestroyObject(Current);
                Current = (Wawe)CreateAndReturnObject(typeof(Wawe), MainPosition);
            }
        }
    }
}
