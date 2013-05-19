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
        float scale = 0.25f;
        Rectangle boxCollider;
        public bool isOnScreen;

        public Terrain(Game1 game, float startAngle)
        {
            this.game = game;
            angle = startAngle;
            texture = game.Content.Load<Texture2D>(@"Earth_fragment");
            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;
            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * scale), (int)(texture.Bounds.Height * scale));

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

           angle -= elapsed * GameSettings.EARTH_ROTATION_SPEED;
            float circle = MathHelper.Pi * 2;
            angle = angle % circle;
            adjustPosition();
            if (isOnScreen)
            {
                detectCollistions();
            }
            
        }

        private void detectCollistions()
        {
            if (boxCollider.Intersects(game.player.boxCollider))
            {

                game.player.adjustPosition(boxCollider);

            }

            if (boxCollider.Intersects(game.bird.boxCollider))
            {

                game.bird = new Bird(game, game.player);

            }
        }

        public virtual void Draw(SpriteBatch batch) 
        {
            batch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0f);
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            //batch.Draw(t, boxCollider, Color.Black);


        }
        

        private void adjustPosition()
        {
            position.X = (int)(game.earth.radius * 1f * (float)Math.Cos(angle) + game.screenWidth * 0.5f);
            position.Y = (int)( game.earth.radius * 1f * (float)Math.Sin(angle) + (game.screenHeight * 0.6f + game.earth.radius));
            boxCollider.X = (int)(position.X -10f);//(int)position.X - boxCollider.Width;
            boxCollider.Y = (int)(position.Y - 10f); //+ boxCollider.Height/3;
            //Console.Out.WriteLine("Terrain Position: X " + position.X + " Y " + position.Y);
            if (position.X < 0 - texture.Width || position.X > game.screenWidth + texture.Width)
            {
                isOnScreen = false;
            }
            else
            {
                isOnScreen = true;
                //Console.Out.WriteLine("Terrain Position: X " + position.X + " Y " + position.Y);
                //Console.Out.WriteLine("Collider Position: X " + boxCollider.X + " Y " + boxCollider.Y);
            }
        }
    }
}
