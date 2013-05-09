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
    public class Terrain
    {
        Texture2D texture;
        public Vector2 position;
        public float angle; // radians
        public Game1 game;
        Vector2 origin;
        float scale = 2f;
        Rectangle boxCollider;

        public Terrain(Game1 game, float startAngle)
        {
            this.game = game;
            angle = startAngle;
            texture = game.Content.Load<Texture2D>(@"Earth_fragment");
            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
           float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            angle -= elapsed * 0.02f;
            float circle = MathHelper.Pi * 2;
            angle = angle % circle;
            adjustPosition();
        }

        public virtual void Draw(SpriteBatch batch) 
        {
            batch.Draw(texture, position, null, Color.White, angle, origin, scale, SpriteEffects.None, 0f);
        }
        

        private void adjustPosition()
        {
            position.X = game.earth.radius * 1f * (float)Math.Cos(angle) + game.screenWidth * 0.5f;
            position.Y = game.earth.radius * 1f * (float)Math.Sin(angle) + (game.screenHeight * 0.5f + game.earth.radius);
            //Console.Out.WriteLine("Terrain Position: X " + position.X + " Y " + position.Y);
        }
    }
}
