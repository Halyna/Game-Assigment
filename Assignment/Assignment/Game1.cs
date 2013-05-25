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

namespace Assignment
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Earth earth;
        public Player player;
        public Bird bird;
        public Fly fly;

        public int screenWidth;
        public int screenHeight;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            screenWidth = 1024;
            screenHeight = 800;
        }

        protected override void Initialize()
        {
            base.Initialize();

            InputManager.Initialize();
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();


            this.earth = new Earth(this);
            this.player = new Player(this);
            this.bird = new Bird(this, player);
            this.fly = new Fly(this);
            
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.Initialize(Content);
            Textures.LoadTextures();
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
            earth.Update(gameTime);
            player.Update(gameTime);
            bird.Update(gameTime);
            fly.Update(gameTime);

            InputManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);
            spriteBatch.Begin();

            earth.Draw(spriteBatch);
            player.Draw(gameTime, spriteBatch);
            bird.Draw(spriteBatch);
            fly.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
            
        }

        internal void GameOver()
        {
            Initialize();
        }
    }
}
