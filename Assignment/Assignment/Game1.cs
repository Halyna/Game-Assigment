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
using System.IO;

namespace Assignment
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum GameState { MainMenu, InGame, GameOver, Paused, GameComplete };

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Earth earth;
        public Player player;

        public Background background;
        public MeteorBig meteorBig;
        //public MeteorSmall meteorSmall;
        //public Bird bird;

        public ObjectSpawner objectSpawner;

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

        public Texture2D whiteScreen; // for explosion
        public float whiteScreenAlpha = 0;
        public int highScore;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            objectSpawner = new ObjectSpawner(this);
            screenWidth = 1024;
            screenHeight = 800;
            loadHighscore();
        }

        protected override void Initialize()
        {
            base.Initialize();

            InputManager.Initialize();
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();

            colorChange = 255 / 2.0f / 60 / 60; // 2 min, 60 frames per sec

            whiteScreen = Content.Load<Texture2D>(@"WhiteScreen");
            Reset();

        }

        void Reset()
        {
            this.earth = new Earth(this);
            this.player = new Player(this);

            this.background = new Background(this);
            this.meteorBig = new MeteorBig(this);
            //this.meteorSmall = new MeteorSmall(this);
            //this.bird = new Bird(this);
            objectSpawner.Reset();

            MediaPlayer.Play(loop1);
            songPlaying = 1;

            greenColor = 255.0f;
            whiteScreenAlpha = 0;
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
            base.Update(gameTime);

            // Get user input
            if (InputManager.PressedBack() == true && this.gameState == GameState.Paused)
                this.gameState = GameState.InGame;
            else if (InputManager.PressedBack() == true && this.gameState == GameState.InGame)
                this.gameState = GameState.Paused;
            else if (InputManager.PressedBack() == true && this.gameState == GameState.MainMenu)
            {
                saveHighscore();
                this.Exit();
            }

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


            earth.Update(gameTime);
            background.Update(gameTime);

            if (gameState == GameState.InGame)
            {
                MediaPlayer.Resume();
                player.Update(gameTime);
                meteorBig.Update(gameTime);
                //meteorSmall.Update(gameTime);
                //bird.Update(gameTime);
                objectSpawner.Update(gameTime);
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
                whiteScreenAlpha += 0.01f;
                Console.WriteLine("alpha " + whiteScreenAlpha);
                meteorBig.Update(gameTime);
            }
            // check for completion
            if (player.scoreDisplay.timeElapsed > GameSettings.PLAY_TIME)
            {
                gameState = GameState.GameComplete;
                highScore = getHighScore();
            }


            InputManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color skyColor = new Color(255.0f / 255, greenColor / 255, 0);
            GraphicsDevice.Clear(skyColor);
            spriteBatch.Begin();

            background.Draw(spriteBatch);

            //////////////////////////////////////////////////////////////////////////////     
            if (gameState == GameState.GameOver)
            {
                earth.Draw(spriteBatch, gameTime);

                spriteBatch.DrawString(Textures.font24, deathReason, new Vector2(300, 100), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                spriteBatch.Draw(Textures.gameOverText, new Rectangle(screenWidth / 2 - Textures.gameOverText.Width / 2,
                        screenHeight / 2 - Textures.gameOverText.Height / 2, Textures.gameOverText.Width, Textures.gameOverText.Height), Color.White);

            }

            //////////////////////////////////////////////////////////////////////////////
            else if (gameState == GameState.MainMenu)
            {
                earth.Draw(spriteBatch, gameTime);

                spriteBatch.Draw(Textures.logoText, new Rectangle(screenWidth / 2 - Textures.gameOverText.Width / 2,
                        screenHeight / 4 - Textures.gameOverText.Height / 2, Textures.gameOverText.Width, Textures.gameOverText.Height), Color.White);

                spriteBatch.DrawString(Textures.font24, "press enter to start", new Vector2(350, 300), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(Textures.font24, "Highscore: " + highScore, new Vector2(screenWidth - 600, screenHeight - 800), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                // credits
                spriteBatch.DrawString(Textures.font24, "Idea and art by Corbin Butler", new Vector2(screenWidth - 800, screenHeight - 150), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(Textures.font24, "Programmed by Halyna Rubashko and Luke Giles", new Vector2(screenWidth - 800, screenHeight - 100), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(Textures.font24, "Music by Albert Gumpl", new Vector2(screenWidth - 800, screenHeight - 50), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }


            //////////////////////////////////////////////////////////////////////////////
            else if (gameState == GameState.GameComplete)
            {
                earth.Draw(spriteBatch, gameTime);

                meteorBig.Draw(spriteBatch, gameTime);

                Color whiteScreenColor = new Color(whiteScreenAlpha, whiteScreenAlpha, whiteScreenAlpha, whiteScreenAlpha);
                spriteBatch.Draw(whiteScreen, Vector2.Zero, whiteScreenColor);

                // labels
                spriteBatch.DrawString(Textures.font24, "Huge meteor hit Earth!", new Vector2(300, 100), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(Textures.font24, "Your species is now extinct...", new Vector2(250, 180), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(Textures.font24, "Your score is " + player.scoreDisplay.currentPoints.ToString(), new Vector2(320, 240), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(Textures.font24, "Come on, you can do better than that!", new Vector2(170, 320), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(Textures.font24, "High Score:" + this.highScore, new Vector2(320, 400), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            //////////////////////////////////////////////////////////////////////////////
            else
            {
                meteorBig.Draw(spriteBatch, gameTime);
                earth.Draw(spriteBatch, gameTime);
                player.Draw(gameTime, spriteBatch);
                //meteorSmall.Draw(spriteBatch, gameTime);                          
                //bird.Draw(gameTime, spriteBatch); 
                objectSpawner.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();
            base.Draw(gameTime);

        }

        internal void GameOver(String reason)
        {
            deathReason = reason;
            gameState = GameState.GameOver;

        }

        public int getHighScore()
        {
            if (player.scoreDisplay.currentPoints > highScore)
            {
                highScore = player.scoreDisplay.currentPoints;
            }
            return highScore;
        }

        // Saves the card data
        private void saveHighscore()
        {
            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter("highscore.txt");
            if (highScore > 0)
            {
                file.WriteLine(highScore);
            }
            file.Close();
        }

        // Loads the card data
        private void loadHighscore()
        {
            // Check to see if there is data to load.
            if (File.Exists("highscore.txt"))
            {
                // Open the file.
                System.IO.StreamReader file = new System.IO.StreamReader("highscore.txt");
                highScore = Convert.ToInt32(file.ReadLine());
                file.Close();
            }
            else
            {
                highScore = 0;
            }
        }
    }
}
