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
        public enum GameState { MainMenu, InGame, GameOver, Paused, GameComplete};

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

        public Song loop1;
        public Song loop2;
        public Song loop3;
        public Song loop4;
        public Song loop5;
        public Song loop6;
        public int songPlaying;

        // will change sky color over game time from yellow to red
        public float colorChange;
        public float greenColor;

        string deathReason;

        public SoundEffect dyingSoundEffect;
        public SoundEffect hitTerrainSoundEffect;
        public SoundEffect birdSpawnedSoundEffect;
        public SoundEffect biteSoundEffect;

        public SoundEffectInstance dyingSound;
        public SoundEffectInstance hitTerrainSound;
        public SoundEffectInstance birdSpawnedSound;
        public SoundEffectInstance biteSound;

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

            colorChange = 255 / 2.0f / 60 / 60; // 2 min, 60 frames per sec

            Reset();
           
        }

        void Reset()
        {
            this.earth = new Earth(this);
            this.player = new Player(this);
            this.bird = new Bird(this);

            this.background = new Background(this);
            this.meteorBig = new MeteorBig(this);
            this.meteorSmall = new MeteorSmall(this);

            MediaPlayer.Play(loop1);
            songPlaying = 1;

            greenColor = 255.0f;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.Initialize(Content);
            Textures.LoadTextures();


            birdSpawnedSoundEffect = Content.Load<SoundEffect>("Sound/Screech");
            hitTerrainSoundEffect = Content.Load<SoundEffect>("Sound/BoneCrush");
            dyingSoundEffect = Content.Load<SoundEffect>("Sound/dyingYell");
            biteSoundEffect = Content.Load<SoundEffect>("Sound/Bite");
            
            dyingSound = dyingSoundEffect.CreateInstance();
            hitTerrainSound = hitTerrainSoundEffect.CreateInstance();
            birdSpawnedSound = birdSpawnedSoundEffect.CreateInstance();
            biteSound = biteSoundEffect.CreateInstance();
            

            loop1 = Content.Load<Song>("Sound/loop1");
            loop2 = Content.Load<Song>("Sound/loop2");
            loop3 = Content.Load<Song>("Sound/loop3");
            loop4 = Content.Load<Song>("Sound/loop4");
            loop5 = Content.Load<Song>("Sound/loop5");
            loop6 = Content.Load<Song>("Sound/loop6");

            MediaPlayer.IsRepeating = true;
            

            // TODO: use this.Content to load your game content here

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Get user input
            if (InputManager.PressedBack() == true && this.gameState == GameState.Paused)
                this.gameState = GameState.InGame;
            else if (InputManager.PressedBack() == true && this.gameState == GameState.InGame)
                this.gameState = GameState.Paused;
            else if (InputManager.PressedBack() == true && this.gameState == GameState.MainMenu)
                this.Exit();

            if (gameState == GameState.MainMenu && InputManager.PressedStart())
            {
                gameState = GameState.InGame;
            }
            if ((gameState == GameState.GameOver || gameState == GameState.GameComplete) && InputManager.PressedStart())
            {
                gameState = GameState.MainMenu;
                Reset();
            }


            if (this.gameState == GameState.Paused)
            {
                MediaPlayer.Pause();
                InputManager.Update(gameTime);
                return;
            }
            base.Update(gameTime);
            earth.Update(gameTime);
            background.Update(gameTime);

            if (gameState == GameState.InGame)
            {
                MediaPlayer.Resume();
                player.Update(gameTime);
                bird.Update(gameTime);
                meteorBig.Update(gameTime);
                meteorSmall.Update(gameTime);

                greenColor -= colorChange;
   
                #region music tracks

                // update music track
                if (player.scoreDisplay.timeElapsed > GameSettings.PLAY_TIME - GameSettings.PLAY_TIME * 1 / 8)
                {
                    if (songPlaying != 6)
                    {
                        MediaPlayer.Play(loop6);
                        songPlaying = 6;
                    }
                }
                else if (player.scoreDisplay.timeElapsed > GameSettings.PLAY_TIME - GameSettings.PLAY_TIME * 2 / 8)
                {
                    if (songPlaying != 5)
                    {
                        MediaPlayer.Play(loop5);
                        songPlaying = 5;
                    }
                }
                else if (player.scoreDisplay.timeElapsed > GameSettings.PLAY_TIME - GameSettings.PLAY_TIME * 3 / 8)
                {
                    if (songPlaying != 4)
                    {
                        MediaPlayer.Play(loop4);
                        songPlaying = 4;
                    }
                }

                else if (player.scoreDisplay.timeElapsed > GameSettings.PLAY_TIME - GameSettings.PLAY_TIME * 4 / 8)
                {
                    if (songPlaying != 3)
                    {
                        MediaPlayer.Play(loop3);
                        songPlaying = 3;
                    }
                }

                else if (player.scoreDisplay.timeElapsed > GameSettings.PLAY_TIME - GameSettings.PLAY_TIME * 6 / 8)
                {
                    if (songPlaying != 2)
                    {
                        MediaPlayer.Play(loop2);
                        songPlaying = 2;
                    }
                }
                #endregion

            }

            if (gameState == GameState.GameComplete)
            {
                meteorBig.Update(gameTime);
            }
            // check for completion
            if (player.scoreDisplay.timeElapsed > GameSettings.PLAY_TIME)
            {
                gameState = GameState.GameComplete;
            }
            
          
            InputManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color skyColor = new Color(255.0f/255, greenColor/255, 0);
            GraphicsDevice.Clear(skyColor);
            spriteBatch.Begin();

            background.Draw(spriteBatch);
            earth.Draw(spriteBatch, gameTime);
      
            if (gameState == GameState.GameOver)
            {
                spriteBatch.DrawString(Textures.font24, deathReason, new Vector2(300, 100), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                spriteBatch.Draw(Textures.gameOverText, new Rectangle(screenWidth / 2 - Textures.gameOverText.Width / 2,
                        screenHeight / 2 - Textures.gameOverText.Height / 2, Textures.gameOverText.Width, Textures.gameOverText.Height), Color.White);

            }
            else if (gameState == GameState.MainMenu)
            {
                spriteBatch.Draw(Textures.logoText, new Rectangle(screenWidth / 2 - Textures.gameOverText.Width / 2,
                        screenHeight / 3 - Textures.gameOverText.Height / 2, Textures.gameOverText.Width, Textures.gameOverText.Height), Color.White);
            }
            else if (gameState == GameState.GameComplete)
            {
                meteorBig.Draw(spriteBatch, gameTime);

                spriteBatch.DrawString(Textures.font24, "Huge meteor hit Earth!", new Vector2(300, 100), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(Textures.font24, "Your species is now extinct...", new Vector2(250, 180), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(Textures.font24, "Your score is " + player.scoreDisplay.currentPoints.ToString(), new Vector2(320, 240), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(Textures.font24, "Come on, you can do better than that!", new Vector2(170, 320), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            else
            {
                meteorBig.Draw(spriteBatch, gameTime);
                player.Draw(gameTime, spriteBatch);
                meteorSmall.Draw(spriteBatch, gameTime);                          
                bird.Draw(gameTime, spriteBatch);              
            }

            spriteBatch.End();
            base.Draw(gameTime);
            
        }

        internal void GameOver(String reason)
        {
            deathReason = reason;
            gameState = GameState.GameOver;
            
        }
    }
}
