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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        Texture2D texture;
        Game1 game;
        Vector2 position;
        Vector2 origin;
        float angle; // radians
        float scale = 0.2f;

        const float MIN_ANGLE = 4.51f;
        const float MAX_ANGLE = 4.9f;

        public Rectangle boxCollider;
        public bool isJumping;
        public TimeSpan jumpingTime;

        public Player(Game1 game)
            : base(game)
        {
            this.game = game;
            angle = MIN_ANGLE;
            this.position = new Vector2();
            position.X = game.earth.radius * (float)Math.Cos(angle) +game.screenWidth * 0.4f;
            position.Y = 0;
            texture = game.Content.Load<Texture2D>(@"lizard");
            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;
            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * scale), (int)(texture.Bounds.Height * scale));
            jumpingTime = new TimeSpan();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (isJumping)
                jumpingTime += gameTime.ElapsedGameTime;

            if (jumpingTime.TotalMilliseconds > GameSettings.JUMP_TIME)
            {
                jumpingTime = new TimeSpan();
                isJumping = false;
            }
            if (InputManager.IsMovingRight())
            {
                angle += GameSettings.PLAYER_SPEED_X;
                if (angle > MAX_ANGLE)
                    angle = MAX_ANGLE;
                //Console.Out.WriteLine("Angle " + angle);
                adjustPosition(Rectangle.Empty);
               
            }
            else if (InputManager.IsMovingLeft())
            {
                angle -= GameSettings.PLAYER_SPEED_X;
                if (angle < MIN_ANGLE)
                    angle = MIN_ANGLE;
               // Console.Out.WriteLine("Angle " + angle);
                adjustPosition(Rectangle.Empty);
            }
            else
            { 
                // drifting back with earth movement
                angle -= 0.0002f;
                if (angle < MIN_ANGLE)
                    angle = MIN_ANGLE;
                Console.Out.WriteLine("Angle " + angle);
                adjustPosition(Rectangle.Empty);
            }

            // jump
            if (InputManager.PressedJump())
            {
                isJumping = true;
            }

            base.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0f);
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
           // batch.Draw(t, boxCollider, Color.Black);
        }

        public void adjustPosition(Rectangle boxCollider)
        {
            position.X = game.earth.radius * (float)Math.Cos(angle) + game.screenWidth * 0.4f;
            if (boxCollider == Rectangle.Empty)
            {

                if (isJumping)
                {
                    position.Y -= GameSettings.PLAYER_SPEED_Y;
                }
                else
                {
                    // freefall
                    position.Y += GameSettings.PLAYER_SPEED_Y;
                }
            }
            else
            {
                
                    position.Y = boxCollider.Top - this.boxCollider.Height;
                
            }
            this.boxCollider.X = (int)position.X;
            this.boxCollider.Y = (int)position.Y;
            //Console.Out.WriteLine("Player Position: X " + position.X + " Y " + position.Y);
        }

    }
}
