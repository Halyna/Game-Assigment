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
    public class BackgroundElement : Microsoft.Xna.Framework.GameComponent
    {
        public Texture2D texture;
        Vector2 position;
        Game1 game;
        public float alpha;
        public float yOffset;
        public float movementOffset;
        public float scale;
        public bool isOnScreen;

        public BackgroundElement(Game1 game, Texture2D texture, float alpha, int startX, float movementOffset, float yOffset)
            : base(game)
        {
            this.game = game;
            this.texture = texture;
            this.alpha = alpha;
            this.position.X = startX;
            this.movementOffset = movementOffset;
            this.yOffset = yOffset;

            scale = 0.25f;
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

        public override void Update(GameTime gameTime)
        {
            position.X -= movementOffset;
            position.Y = (int)(game.screenHeight * yOffset);

            if (position.X < 0 - texture.Width*scale)
            {
                position.X = texture.Width * scale * 3;
            }


        }

        public void adjustPosition()
        {
           
        }

        public virtual void Draw(SpriteBatch batch)
        {
            Color c = new Color(0, 0, 0, alpha);
            //c.A = (byte) alpha;
            batch.Draw(texture, position, null, c, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
