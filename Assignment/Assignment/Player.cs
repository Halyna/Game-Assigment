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
        Vector2 origin;
        float angle; // radians
        float scale = 0.3f;
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

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            IdleAnimation = Textures.PlayerIdleAnimation;
            MovingAnimation = Textures.PlayerMoveingAnimation;
            JumpingAnimation = Textures.PlayerJumpingAnimation;
            CrouchAnimation = Textures.PlayerCrouchAnimation;
            PlayerAnimationController.PlayAnimation(IdleAnimation);
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            PlayerAnimationController.Update(gameTime);

            if (isJumping)
                jumpingTime += gameTime.ElapsedGameTime;

            if (jumpingTime.TotalMilliseconds > GameSettings.JUMP_TIME)
            {
                jumpingTime = new TimeSpan();
                isJumping = false;
            }

            if (isCrouching)
                crouchingTime += gameTime.ElapsedGameTime;

            if (crouchingTime.TotalMilliseconds > GameSettings.JUMP_TIME)
            {
                crouchingTime = new TimeSpan();
                isCrouching = false;
            }

            if (InputManager.IsMovingRight())
            {
                angle += GameSettings.PLAYER_SPEED_X;
                if (angle > MAX_ANGLE)
                    angle = MAX_ANGLE;
                adjustPosition(Rectangle.Empty);
                isIdle = false;
               
            }
            else if (InputManager.IsMovingLeft())
            {
                angle -= GameSettings.PLAYER_SPEED_X;
                if (angle < MIN_ANGLE)
                    angle = MIN_ANGLE;
                adjustPosition(Rectangle.Empty);
                isIdle = false;
            }
            else
            { 
                // drifting back with earth movement
                angle -= 0.0002f;
                if (angle < MIN_ANGLE)
                    angle = MIN_ANGLE;
                adjustPosition(Rectangle.Empty);
                isIdle = true;
            }

            // jump
            if (InputManager.IsMovingUp() && !isJumping)
            {
                isJumping = true;
                PlayerAnimationController.PlayAnimation(JumpingAnimation);
            }

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

        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {

            PlayerAnimationController.Draw(gameTime, batch, position, scale, SpriteEffects.None, Color.DarkOliveGreen, 0, origin);
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
          //batch.Draw(t, boxCollider, Color.Black);
        }

        public void adjustPosition(Rectangle boxCollider)
        {
            position.X = game.earth.radius * (float)Math.Cos(angle) + game.screenWidth * 0.4f;
            if (boxCollider == Rectangle.Empty)
            {
                if (isJumping)
                    position.Y -= GameSettings.PLAYER_SPEED_Y;
                else
                    position.Y += GameSettings.PLAYER_SPEED_Y;
            }
            else
            {
                position.Y = boxCollider.Top - this.boxCollider.Height;
            }
            this.boxCollider.X = (int)position.X;
            this.boxCollider.Y = (int)position.Y;
            //Console.Out.WriteLine("Player Position: X " + position.X + " Y " + position.Y);
        }


        internal void birdCollided(Bird bird)
        {
            game.bird = new Bird(game, this);
        }

        internal void flyCollided(Fly fly)
        {
            game.fly = new Fly(game);
        }
    }
}
