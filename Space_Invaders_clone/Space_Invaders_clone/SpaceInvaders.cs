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
        SpriteFont DebuggConsoleFont;

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
            graphics.PreferredBackBufferWidth = 64 + 557 + 72;
            graphics.PreferredBackBufferHeight = 64 + 348 + 174 + 52;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Riemer's 2D XNA Tutorial";

            //Initialize the static classes:
            ObjectManager.Initialize();

            //Initialize the debug console:
            Console = new DebugConsole(spriteBatch, new Vector2(0, 0));
            Console.Font = DebuggConsoleFont;

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

            //Initialize default sprites:
            int width = 40;
            int height = 25;
            TextureContainer.DefaultTextures[typeof(InvaderType1)] = TextureContainer.ColoredRectangle(Color.Red, width, height);
            TextureContainer.DefaultTextures[typeof(InvaderType2)] = TextureContainer.ColoredRectangle(Color.Yellow, width, height);
            TextureContainer.DefaultTextures[typeof(InvaderType3)] = TextureContainer.ColoredRectangle(Color.Blue, width, height);
            TextureContainer.DefaultTextures[typeof(EnemyBullet)] = TextureContainer.ColoredRectangle(Color.Blue, 8, 16);
            TextureContainer.DefaultTextures[typeof(Player)] = TextureContainer.ColoredRectangle(Color.Green, 53, 32);
            TextureContainer.DefaultTextures[typeof(PlayerBullet)] = TextureContainer.ColoredRectangle(Color.Green, 5, 13);
            TextureContainer.DefaultTextures[typeof(GameOverSign)] = TextureContainer.AddTextureAndReturn("Game over");

            //Create game objects:
            ObjectManager.InstantCreate(typeof(Invasion), new Vector2(64, 64));
            ObjectManager.InstantCreate(typeof(Player), new Vector2(Device.Viewport.Width / 2, Device.Viewport.Height - 32 - 10)); 
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            ObjectManager.UpdateAll();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            ObjectManager.DrawAll();
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
