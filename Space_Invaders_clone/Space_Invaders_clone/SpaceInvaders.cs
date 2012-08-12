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
using Debugging;

namespace Space_Invaders_clone
{
    public class SpaceInvaders : Microsoft.Xna.Framework.Game
    {
        //Basic game info:
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static GraphicsDevice Device;

        //The debug console:
        public static DebugConsole Console;

        //References:
        public static Score RefScore;
        public static LifeBar RefLife;

        public SpaceInvaders()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Device = graphics.GraphicsDevice;

            //Initialize game info:
            graphics.PreferredBackBufferWidth = 620;
            graphics.PreferredBackBufferHeight = 500;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Space Invaders by Sandi Dusic";

            //Initialize the static classes:
            ObjectManager.Initialize();

            //Initialize the debug console:
            Console = new DebugConsole(spriteBatch, new Vector2(0, 0));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Initialize the GameInfo:
            GameInfo.RefSpriteBatch = spriteBatch;
            GameInfo.RefDevice = Device;
            GameInfo.RefDeviceManager = graphics;
            GameInfo.RefContent = Content;
            GameInfo.RefConsole = Console;

            //Initialize the texture container:
            TextureContainer.Initialize();

            //Load fonts:
            Console.Font = Content.Load<SpriteFont>("DebuggConsoleFont");
            Score.Font = Content.Load<SpriteFont>("Score");

            //Initialize default sprites:
            int width = 40;
            int height = 25;
            TextureContainer.DefaultTextures[typeof(InvaderType1)] = TextureContainer.ColoredRectangle(Color.Red, width, height);
            TextureContainer.DefaultTextures[typeof(InvaderType2)] = TextureContainer.ColoredRectangle(Color.Yellow, width, height);
            TextureContainer.DefaultTextures[typeof(InvaderType3)] = TextureContainer.ColoredRectangle(Color.Blue, width, height);
            TextureContainer.DefaultTextures[typeof(SpecialInvader)] = TextureContainer.ColoredRectangle(Color.Orange, 45, 20);
            TextureContainer.DefaultTextures[typeof(EnemyBullet)] = TextureContainer.ColoredRectangle(Color.Blue, 8, 16);
            TextureContainer.DefaultTextures[typeof(Player)] = TextureContainer.ColoredRectangle(Color.Green, 50, 30);
            TextureContainer.DefaultTextures[typeof(PlayerBullet)] = TextureContainer.ColoredRectangle(Color.Green, 5, 13);
            TextureContainer.DefaultTextures[typeof(GameOverSign)] = TextureContainer.AddTextureAndReturn("Game over");
            TextureContainer.DefaultTextures[typeof(Block)] = TextureContainer.ColoredRectangle(Color.LightGreen, 20, 20);

            //Load sounds:
            Player.ShootSound = Content.Load<SoundEffect>("Laser");
            BaseInvader.ShootSound = Content.Load<SoundEffect>("Laser");
            BaseInvader.MoveSound = Content.Load<SoundEffect>("Invader move");
            BaseInvader.ExplodeSound = Content.Load<SoundEffect>("Explosion");
            SpecialInvader.ExplodeSound = Content.Load<SoundEffect>("Explosion");
            LifeBar.PlayerHit = Content.Load<SoundEffect>("PlayerHit");

            //Create game objects:
            ObjectManager.InstantCreate(typeof(Invasion), new Vector2(90, 85));
            ObjectManager.InstantCreate(typeof(Player), new Vector2(Device.Viewport.Width / 2 - 25, 460));
            BlockMaker.MakeANewSetOfBlocks();
            ObjectManager.InstantCreate(typeof(Score), new Vector2(20, 15));
            ObjectManager.InstantCreate(typeof(LifeBar), new Vector2(386, 15));
            ObjectManager.InstantCreate(typeof(SpecialInvaderSpawner), new Vector2(0, 0));

            //Make a reference to the UI objects: (so that other objects can interact with them)
            RefScore = (Score)ObjectManager.Get(typeof(Score))[0];
            RefLife = (LifeBar)ObjectManager.Get(typeof(LifeBar))[0];
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            ObjectManager.UpdateAll();
            Console.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            ObjectManager.DrawAll();
            Console.WriteConsole();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
