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
    
    public class Fly : Microsoft.Xna.Framework.GameComponent
    {

        Texture2D texture;
        Game1 game;
        public Terrain terrain;

        Vector2 position;
        Vector2 origin;
        float scale;
        float rotationAngle; // radians
        float positionAngle; // radians
        public Rectangle boxCollider;

        public Fly(Game1 game, float startAngle, Terrain terrain)
            : base(game)
        {
            this.game = game;
            this.terrain = terrain;

            this.position = new Vector2();
            Random r = new Random();
            scale = 0.05f;
            //position.X = r.Next(game.screenWidth);
            position.Y = game.screenHeight * 0.5f;
            positionAngle = startAngle;
            texture = game.Content.Load<Texture2D>(@"Fly");
            origin.X = texture.Width * 2 * scale;
            origin.Y = terrain.position.Y - 100;
            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * scale), (int)(texture.Bounds.Height * scale));
            Initialize();
        }

       
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
            base.Update(gameTime);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            positionAngle -= elapsed * GameSettings.EARTH_ROTATION_SPEED;

            rotationAngle += 0.1f;
            origin.X = (int)(game.earth.radius * 1f * (float)Math.Cos(positionAngle) + game.screenWidth * 0.5f);
            origin.Y = terrain.position.Y - game.player.boxCollider.Height * 2.5f;
            position = origin + Vector2.Transform(new Vector2(20, 0), Matrix.CreateRotationZ(rotationAngle));

            this.boxCollider.X = (int)position.X;
            this.boxCollider.Y = (int)position.Y;

            detectCollistions();
        }

        private void detectCollistions()
        {
            if (boxCollider.Intersects(game.player.boxCollider))
            {

                game.player.flyCollided(this);

            }
        }
        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0f);
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            //batch.Draw(t, boxCollider, Color.Black);


        }
    }
}
