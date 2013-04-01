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

        public Player(Game1 game)
            : base(game)
        {
            this.game = game;
            angle = MIN_ANGLE;
            this.position = new Vector2();
            position.X = game.earth.radius * (float)Math.Cos(angle) +game.screenWidth * 0.4f;
            position.Y = game.earth.radius * (float)Math.Sin(angle) + (game.screenHeight * 0.35f + game.earth.radius);
            texture = game.Content.Load<Texture2D>(@"lizard");
            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;
           
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
            if (InputManager.IsMovingRight())
            {
                angle +=0.005f;
                if (angle > MAX_ANGLE)
                    angle = MAX_ANGLE;
                Console.Out.WriteLine("Angle " + angle);
                adjustPosition();
               
            }
            else if (InputManager.IsMovingLeft())
            {
                angle -= 0.005f;
                if (angle < MIN_ANGLE)
                    angle = MIN_ANGLE;
                Console.Out.WriteLine("Angle " + angle);
                adjustPosition();
            }
            else
            { 
                // drifting back with earth movement
                angle -= 0.0003f;
                if (angle < MIN_ANGLE)
                    angle = MIN_ANGLE;
                Console.Out.WriteLine("Angle " + angle);
                adjustPosition();
            }

            

            base.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0f);
        }

        private void adjustPosition()
        { 
            position.X = game.earth.radius * (float)Math.Cos(angle) + game.screenWidth * 0.4f;
            position.Y = game.earth.radius * (float)Math.Sin(angle) + (game.screenHeight * 0.35f + game.earth.radius);
            Console.Out.WriteLine("X " + position.X);
            Console.Out.WriteLine("Y " + position.Y);
        }

    }
}
