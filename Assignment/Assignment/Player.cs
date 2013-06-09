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
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        public const float colliderOffsetX = 0.45f;
        public const float colliderOffsetY = 0.6f;

        Texture2D texture;
        Game1 game;
        public ScoreDisplay scoreDisplay;

        public Vector2 position;

        public bool isStuckRight;
        public double isStuckLeft; //will use gameTime as a reference for multiple terrain colliders per frame

        Vector2 origin;
        float angle; // radians
        float scale = 0.25f;
        const float MIN_ANGLE = 4.635f;
        const float MAX_ANGLE = 4.82f;

        public Rectangle boxCollider;

        // States
        public bool isJumping;
        public bool isIdle;
        public bool isCrouching;
        public bool inTheAir;
        public TimeSpan jumpingTime;
        public TimeSpan crouchingTime;

        // Animations
        private Animation IdleAnimation;
        private Animation MovingAnimation;
        private Animation JumpingAnimation;
        private Animation CrouchAnimation;
        private AnimationController PlayerAnimationController;

        public Player(Game1 game)
            : base(game)
        {
            this.game = game;
            scoreDisplay = new ScoreDisplay(game);
            scoreDisplay.currentSize = (int)(scale * 1000);

            angle = MIN_ANGLE;
            this.position = new Vector2();
            position.X = game.earth.radius * (float)Math.Cos(angle) + game.screenWidth * 0.4f;
            position.Y = 100;

            texture = game.Content.Load<Texture2D>(@"PlayerAnimations/Idle/t_IDLE_0");
            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;
            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * scale * colliderOffsetX), (int)(texture.Bounds.Height * scale * colliderOffsetY));
            jumpingTime = new TimeSpan();
            crouchingTime = new TimeSpan();

            Initialize();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Initialize()
        {
            IdleAnimation = Textures.PlayerIdleAnimation;
            MovingAnimation = Textures.PlayerMoveingAnimation;
            JumpingAnimation = Textures.PlayerJumpingAnimation;
            CrouchAnimation = Textures.PlayerCrouchAnimation;
            PlayerAnimationController.PlayAnimation(IdleAnimation);
            base.Initialize();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Update(GameTime gameTime)
        {

            PlayerAnimationController.Update(gameTime);

            // Movement updates
            if (isJumping)
                jumpingTime += gameTime.ElapsedGameTime;

            if (jumpingTime.TotalMilliseconds > GameSettings.JUMP_TIME)
            {
                jumpingTime = new TimeSpan();
                isJumping = false;
            }

            if (InputManager.IsMovingRight() && !isCrouching && !isStuckRight)
            {
                angle += GameSettings.PLAYER_SPEED_X;
                if (angle > MAX_ANGLE)
                    angle = MAX_ANGLE;

                isIdle = false;

            }
            else if (InputManager.IsMovingLeft() && !isCrouching && isStuckLeft == 0)
            {
                angle -= GameSettings.PLAYER_SPEED_X;
                if (angle < MIN_ANGLE)
                    if (!isStuckRight)
                    {
                        angle = MIN_ANGLE;
                    }
                    else
                    {
                        // we are dead!
                        gameOver("Earth is moving too fast for you...");
                    }

                isIdle = false;
            }
            else
            {
                // drifting back with earth movement
                angle -= GameSettings.EARTH_ROTATION_SPEED / 60;
                if (angle < MIN_ANGLE)
                {
                    if (!isStuckRight)
                    {
                        angle = MIN_ANGLE;
                    }
                    
                }
                    
                isIdle = true;
            }

            if (InputManager.IsMovingUp() && !isJumping && !inTheAir)
            {
                isJumping = true;
                //add jump sound
                PlayerAnimationController.PlayAnimation(JumpingAnimation);
            }
            if (InputManager.IsMovingDown() && !isCrouching && !inTheAir)
            {
                isCrouching = true;
                //add crouch sound
            }
            else if (!InputManager.IsMovingDown())
            {
                isCrouching = false;
            }

            adjustPosition(Rectangle.Empty, gameTime);
            adjustCollider();

            // Play animation
            if (isCrouching)
            {
                PlayerAnimationController.PlayAnimation(CrouchAnimation);
            }

            if (isIdle && !isJumping && !isCrouching)
            {
                PlayerAnimationController.PlayAnimation(IdleAnimation);
            }
            else if (!isJumping && !isCrouching)
                PlayerAnimationController.PlayAnimation(MovingAnimation);

            scoreDisplay.Update(gameTime);

            // are we too small to continue?
            if (scale < 0.2f) 
            {
                gameOver("Your poor lizard died of hunger...");
            }
            if (angle < MIN_ANGLE - 0.02)
            {
                gameOver("Earth is moving too fast for you...");
            }

            base.Update(gameTime);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void gameOver(String reason)
        {
            
            if (game.dyingSound.State != SoundState.Playing)
            {
               game.dyingSound.Play();
            }

            game.GameOver(reason);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {

            PlayerAnimationController.Draw(gameTime, batch, position, scale, SpriteEffects.None, Color.DarkOliveGreen, 0, Vector2.Zero);
            scoreDisplay.Draw(batch);

            /* debug: collider

            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            Color c = new Color(0, 0, 0, 0.5f);
            batch.Draw(t, boxCollider, c);
             */
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void adjustPosition(Rectangle boxCollider, GameTime gameTime)
        {
            if (boxCollider == Rectangle.Empty)
            {
                inTheAir = true;
                isStuckRight = false;
                isStuckLeft = 0;


                if (isJumping)
                {
                    position.Y -= GameSettings.PLAYER_SPEED_Y * (3.0f / ((int)(jumpingTime.TotalMilliseconds / 100) + 1));
                    //Console.WriteLine("Jump speed " + 3.0f/((int)(jumpingTime.TotalMilliseconds / 100) + 1));
                }
                else
                    position.Y += GameSettings.PLAYER_SPEED_Y;

                if (position.Y > game.screenHeight)
                    gameOver("Earth is moving too fast for you...");
            }
            else
            {
                inTheAir = false;
                if (position.Y <= boxCollider.Top - 30)
                {

                    position.Y = boxCollider.Top - this.boxCollider.Height -
                        (int)((texture.Bounds.Height * scale - this.boxCollider.Height) / 2);

                    isStuckRight = false;
                    if (gameTime.TotalGameTime.TotalMilliseconds != isStuckLeft)
                        isStuckLeft = 0;

                }
                else // we are facing a wall, stop movement
                {
                    if (position.X < boxCollider.X)
                        isStuckRight = true;
                    else if (position.X > boxCollider.X)
                        isStuckLeft = gameTime.TotalGameTime.TotalMilliseconds;
                }

               // }
            }

            position.X = game.earth.radius * (float)Math.Cos(angle) + game.screenWidth * 0.4f;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void adjustCollider()
        {
            boxCollider.Width = (int)(texture.Bounds.Width * scale * colliderOffsetX);
            boxCollider.Height = (int)(texture.Bounds.Height * scale * colliderOffsetY);
            this.boxCollider.X = (int)position.X + (int)((texture.Bounds.Width * scale - boxCollider.Width) / 2);
            this.boxCollider.Y = (int)position.Y + (int)((texture.Bounds.Height * scale - boxCollider.Height) / 2);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        internal void birdCollided(Bird bird)
        {
            if (isCrouching)
                return;
            //game.bird = new Bird(game);
            game.objectSpawner.birds.Remove(bird);
            scale -= 0.01f;

            scoreDisplay.currentPoints -= 100;
            if (scoreDisplay.currentPoints < 0)
            {
                scoreDisplay.currentPoints = 0;
            }
            scoreDisplay.currentSize = (int)(scale * 1000);
            
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        internal void flyCollided(Fly fly)
        {
            if (isCrouching)
                return;
            fly.terrain.fly = null;
            
            scale += 0.01f;

            scoreDisplay.currentPoints += 100;
            scoreDisplay.currentSize = (int)(scale * 1000);

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        internal void FallInPit(Rectangle pitCollider)
        {
            position.Y = boxCollider.Bottom - this.boxCollider.Height;
            gameOver("Lava will kill you every time...");
        }

        internal void MeteorCollided(MeteorSmall meteorSmall)
        {
            if (isCrouching)
                return;
            //game.meteorSmall = new MeteorSmall(game);
            //game.objectSpawner.meteors.Remove(meteorSmall);
            meteorSmall.Animate();
            scale -= 0.005f;

            scoreDisplay.currentPoints -= 50;
            if (scoreDisplay.currentPoints < 0)
            {
                scoreDisplay.currentPoints = 0;
            }
            scoreDisplay.currentSize = (int)(scale * 1000);
        }
    }
}
