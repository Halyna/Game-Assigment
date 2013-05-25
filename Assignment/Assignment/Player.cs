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
        Texture2D texture;
        Game1 game;

        public Vector2 position;
        public bool isStuckRight;
        public double isStuckLeft;
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
        public TimeSpan jumpingTime;
        public TimeSpan crouchingTime;

        // Animations
        private Animation IdleAnimation;
        private Animation MovingAnimation;
        private Animation JumpingAnimation;
        private Animation CrouchAnimation;
        private AnimationPlayer PlayerAnimationController;

        public Player(Game1 game)
            : base(game)
        {
            this.game = game;
            angle = MIN_ANGLE;
            this.position = new Vector2();
            position.X = game.earth.radius * (float)Math.Cos(angle) + game.screenWidth * 0.4f;
            position.Y = 100;
            texture = game.Content.Load<Texture2D>(@"PlayerAnimations/Idle/t_IDLE_0");
            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;
            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * scale * 0.7), (int)(texture.Bounds.Height * scale* 0.7));
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

            if (isCrouching)
            {
                crouchingTime += gameTime.ElapsedGameTime;
                // drifting back with earth movement
                angle -= 0.0002f;
                if (angle < MIN_ANGLE)
                    angle = MIN_ANGLE;
            }
                // Read input
            else
            {
                if (InputManager.IsMovingRight() && !isStuckRight)
                {
                    angle += GameSettings.PLAYER_SPEED_X;
                    if (angle > MAX_ANGLE)
                        angle = MAX_ANGLE;

                    isIdle = false;

                }
                else if (InputManager.IsMovingLeft() && isStuckLeft == 0)
                {
                    angle -= GameSettings.PLAYER_SPEED_X;
                    if (angle < MIN_ANGLE)
                        angle = MIN_ANGLE;
                    isIdle = false;
                }
                else
                {
                    // drifting back with earth movement
                    angle -= 0.0002f;
                    if (angle < MIN_ANGLE)
                        if (!isStuckRight)
                        {
                            angle = MIN_ANGLE;
                        }
                        else
                        {
                             // we are dead!
                            gameOver();
                        }
                    isIdle = true;
                }

                if (InputManager.IsMovingUp() && !isJumping)
                {
                    isJumping = true;
                    PlayerAnimationController.PlayAnimation(JumpingAnimation);
                }

            }
            if (crouchingTime.TotalMilliseconds > GameSettings.JUMP_TIME)
            {
                crouchingTime = new TimeSpan();
                isCrouching = false;
            }

           
            adjustPosition(Rectangle.Empty, gameTime);
            adjustCollider();

            // Play animation
            if (InputManager.IsMovingDown() && !isCrouching)
            {
                isCrouching = true;
                PlayerAnimationController.PlayAnimation(CrouchAnimation);
            }

            if (isIdle && !isJumping && !isCrouching)
            {
                 PlayerAnimationController.PlayAnimation(IdleAnimation);
            }
            else if (!isJumping && !isCrouching)
                PlayerAnimationController.PlayAnimation(MovingAnimation);

            base.Update(gameTime);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void gameOver()
        {
            game.GameOver();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {

            PlayerAnimationController.Draw(gameTime, batch, position, scale, SpriteEffects.None, Color.DarkOliveGreen, 0, origin);
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
          //batch.Draw(t, boxCollider, Color.Black);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void adjustPosition(Rectangle boxCollider, GameTime gameTime)
        {
            
            if (boxCollider == Rectangle.Empty)
            {
                isStuckRight = false;
                isStuckLeft = 0;

                if (isJumping)
                    position.Y -= GameSettings.PLAYER_SPEED_Y;
                else
                    position.Y += GameSettings.PLAYER_SPEED_Y;

                if (position.Y > game.screenHeight)
                    gameOver();
            }
            else
            {
               // Console.WriteLine("box collider top {0}, player top {1}, player bottom {2}", boxCollider.Top, position.Y, position.Y + this.boxCollider.Height);
                if (position.Y <= boxCollider.Top)
                { 
                    position.Y = boxCollider.Top - this.boxCollider.Height;
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
            }

            position.X = game.earth.radius * (float)Math.Cos(angle) + game.screenWidth * 0.4f;
            
            //Console.Out.WriteLine("Player Position: X " + position.X + " Y " + position.Y);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void adjustCollider()
        {
            boxCollider.Width = (int)(texture.Bounds.Width * scale * 0.6);
            boxCollider.Height = (int)(texture.Bounds.Height * scale * 0.7);
            this.boxCollider.X = (int)position.X;
            this.boxCollider.Y = (int)position.Y;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        internal void birdCollided(Bird bird)
        {
            if (isCrouching)
                return;
            game.bird = new Bird(game, this);
            scale -= 0.01f;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        internal void flyCollided(Fly fly)
        {
            if (isCrouching)
                return;
            game.fly = new Fly(game);
            scale += 0.01f;
        }
    }
}
