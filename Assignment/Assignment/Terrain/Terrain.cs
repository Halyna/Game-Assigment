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
        public float scale = 0.2f;
        public Rectangle boxCollider;
        public bool isOnScreen;
        public float offsetAngle;

        public Fly fly;

        public Terrain(Game1 game, float startAngle, bool hasFly)
        {
            this.game = game;
            angle = startAngle;

            Random r = new Random();
            float flyOffset = (float)r.NextDouble() * 0.055f; // max offfset angle
            if (hasFly)
            {
                fly = new Fly(game, startAngle + flyOffset, this);
            }

            isOnScreen = false;
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
            if (isOnScreen && game.gameState == Game1.GameState.InGame)
            {
                detectCollistions(gameTime);
            }

            if (fly != null && game.gameState != Game1.GameState.GameComplete)
            {
                fly.Update(gameTime);
            }
            
        }

        protected virtual void detectCollistions(GameTime gameTime)
        {
            if (boxCollider.Intersects(game.player.boxCollider))
            {
                //Console.WriteLine("Terrain collision {0}", this.ToString());
                game.player.adjustPosition(boxCollider, gameTime);

            }

            for (int i = game.objectSpawner.birds.Count - 1; i >= 0; i--)
            {
                if (boxCollider.Intersects(game.objectSpawner.birds[i].boxCollider))
                {
                    game.objectSpawner.birds[i].FlyAway();
                }
            }

            for (int i = game.objectSpawner.meteors.Count - 1; i >= 0; i--)
            {
                if (boxCollider.Intersects(game.objectSpawner.meteors[i].boxCollider))
                {
                    game.objectSpawner.meteors.RemoveAt(i);
                }
            }
           
        }

        public virtual void Draw(SpriteBatch batch, GameTime gameTime) 
        {
            batch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0f);
            if (fly != null && game.gameState != Game1.GameState.GameComplete)
            {
                fly.Draw(batch, gameTime);
            }

            /* debug: collider
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            Color c = new Color(0, 0, 0, 0.5f);
            batch.Draw(t, boxCollider, c);
            */

        }
        

        public virtual void adjustPosition()
        {
            
        }
    }
}
