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
    public class Score : GameObject
    {
        public Score()
            : base()
        {
        }
        public Score(Vector2 position)
            : base(position)
        {
        }

        public static SpriteFont Font;
        private int amount = 0;

        public void AddPoints(int points)
        {
            amount += points;

            //Create an score pop up 
            float lenghtOfScoreString = Font.MeasureString(amount.ToString()).X;
            Vector2 afterScore = new Vector2(Sprite.Position.X + lenghtOfScoreString + 10, Sprite.Position.Y);
            ScorePopUp popUp = (ScorePopUp)CreateAndReturnObject(typeof(ScorePopUp), afterScore);
            popUp.Amount = points;
        }

        public override void Draw()
        {
            GameInfo.RefSpriteBatch.DrawString(Font, amount.ToString(), Sprite.Position, Color.Yellow);
        }
    }
}
