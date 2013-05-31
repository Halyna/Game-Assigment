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

        public Fly fly;

        public Terrain(Game1 game, float startAngle, bool hasFly)
        {
            this.game = game;
            angle = startAngle;
            if (hasFly)
            {
                fly = new Fly(game, startAngle, this);
            }
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

            if (fly != null)
            {
                fly.Update(gameTime);
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

                game.bird.FlyAway();

            }
        }

        public virtual void Draw(SpriteBatch batch) 
        {
            batch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0f);
            if (fly != null)
            {
                fly.Draw(batch);
            }

            // debug: collider
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
