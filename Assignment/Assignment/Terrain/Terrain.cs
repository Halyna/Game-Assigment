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
        public Texture2D texture;
        public Vector2 position;
        public float angle; // radians
        public Game1 game;
        public Vector2 origin;
        public float scale = 0.25f;
        public Rectangle boxCollider;
        public bool isOnScreen;

        public Terrain(Game1 game, float startAngle)
        {
            this.game = game;
            angle = startAngle;
        }
       
        public virtual void Initialize()
        {
            // TODO: Add your initialization code here
        }

      
        public virtual void Update(GameTime gameTime)
        {
           float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

           angle -= elapsed * GameSettings.EARTH_ROTATION_SPEED;
            float circle = MathHelper.Pi * 2;
            angle = angle % circle;
            adjustPosition();
            if (isOnScreen)
            {
                detectCollistions(gameTime);
            }
            
        }

        private void detectCollistions(GameTime gameTime)
        {
            if (boxCollider.Intersects(game.player.boxCollider))
            {
                //Console.WriteLine("Terrain collision {0}", this.ToString());
                game.player.adjustPosition(boxCollider, gameTime);

            }

            if (boxCollider.Intersects(game.bird.boxCollider))
            {

                //sound of bird hitting ground
                game.bird = new Bird(game, game.player);

            }
        }

        public virtual void Draw(SpriteBatch batch) 
        {
            batch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0f);
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            Color c = new Color(0, 0, 0, 0.5f);
            batch.Draw(t, boxCollider, c);


        }
        

        public virtual void adjustPosition()
        {
            
        }
    }
}
