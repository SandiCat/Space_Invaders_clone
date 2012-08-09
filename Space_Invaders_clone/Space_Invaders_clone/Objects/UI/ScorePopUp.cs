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
    public class ScorePopUp : GameObject
    {
        public ScorePopUp()
            : base()
        {
        }
        public ScorePopUp(Vector2 position)
            : base(position)
        {
        }

        public static SpriteFont Font;

        public int Amount;

        public override void Create(GameObject createdObject)
        {
            if (createdObject == this)
            {
                Font = Score.Font;
                Solid = false;
                Visable = false;
                ChangeSpriteTexture(TextureContainer.ColoredRectangle(Color.White, 1, 18));
            }
        }
        public override void Draw()
        {
            GameInfo.RefSpriteBatch.DrawString(Font, "+ " + Amount.ToString(), Sprite.Position, Color.Red);
        }
        public override void Update()
        {
            StepAngle(Directions.Up, 1);
        }
        public override void OutsideOfWindow()
        {
            DestroyObject(this);
        }
    }
}
