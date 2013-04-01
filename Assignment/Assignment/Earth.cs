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
    public class Earth : Microsoft.Xna.Framework.GameComponent
    {
        Texture2D texture;
        Game game;
        Vector2 position;
        Vector2 origin;
        float rotationAngle;
        float scale = 3.0f;
        public float radius;

        public Earth(Game1 game)
            : base(game)
        {
            this.game = game;
            this.position = new Vector2(game.screenWidth * 0.5f, game.screenHeight * 3.5f);
            texture = game.Content.Load<Texture2D>(@"Earth");
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
            radius = texture.Bounds.Height * 0.5f * scale;
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
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your game logic here.
            rotationAngle -= elapsed * 0.02f;
            float circle = MathHelper.Pi * 2;
            rotationAngle = rotationAngle % circle;

            base.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch batch) 
        {
            batch.Draw(texture, position, null, Color.White, rotationAngle, origin, scale, SpriteEffects.None, 0f);

        }
    }
}
