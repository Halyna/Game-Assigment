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
        public enum GameState { MainMenu, InGame, GameOver };

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Earth earth;
        public Player player;
        public Bird bird;
        public Background background;
        public MeteorBig meteorBig;
        public MeteorSmall meteorSmall;

        public int screenWidth;
        public int screenHeight;

        public GameState gameState = GameState.MainMenu;


        public SoundEffect dyingSound;
        public SoundEffect hitTerrainSound;
        public SoundEffect birdSpawnedSound;
        public SoundEffect biteSound;

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
            this.bird = new Bird(this);
            this.background = new Background(this);
            this.meteorBig = new MeteorBig(this);
            this.meteorSmall = new MeteorSmall(this);
            
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.Initialize(Content);
            Textures.LoadTextures();

            birdSpawnedSound = Content.Load<SoundEffect>("Sound/Screech");
            hitTerrainSound = Content.Load<SoundEffect>("Sound/BoneCrush");
            dyingSound = Content.Load<SoundEffect>("Sound/Dying");
            biteSound = Content.Load<SoundEffect>("Sound/Bite");

            // TODO: use this.Content to load your game content here

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (InputManager.PressedBack() == true)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
            earth.Update(gameTime);
            background.Update(gameTime);
            if (gameState == GameState.InGame)
            {
                player.Update(gameTime);
                bird.Update(gameTime);
                meteorBig.Update(gameTime);
                meteorSmall.Update(gameTime);
            }

            

            if (gameState == GameState.MainMenu && InputManager.PressedStart())
            {
                gameState = GameState.InGame;
            }
            if (gameState == GameState.GameOver && InputManager.PressedStart())
            {
                gameState = GameState.MainMenu;
                Initialize();
            }

            InputManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Yellow);
            spriteBatch.Begin();

            background.Draw(spriteBatch);
            earth.Draw(spriteBatch, gameTime);
            
            

            if (gameState == GameState.GameOver)
            {
                spriteBatch.Draw(Textures.gameOverText, new Rectangle(screenWidth / 2 - Textures.gameOverText.Width / 2,
                        screenHeight / 2 - Textures.gameOverText.Height / 2, Textures.gameOverText.Width, Textures.gameOverText.Height), Color.White);

            }
            else if (gameState == GameState.MainMenu)
            {
                spriteBatch.Draw(Textures.logoText, new Rectangle(screenWidth / 2 - Textures.gameOverText.Width / 2,
                        screenHeight / 3 - Textures.gameOverText.Height / 2, Textures.gameOverText.Width, Textures.gameOverText.Height), Color.White);
            }

            else
            {
                bird.Draw(gameTime, spriteBatch);
                player.Draw(gameTime, spriteBatch);
                meteorBig.Draw(spriteBatch, gameTime);
                meteorSmall.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();
            base.Draw(gameTime);
            
        }

        internal void GameOver()
        {
            dyingSound.Play();
            gameState = GameState.GameOver;
            
        }
    }
}
