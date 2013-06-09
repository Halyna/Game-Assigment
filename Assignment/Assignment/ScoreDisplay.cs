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
    public class ScoreDisplay : Microsoft.Xna.Framework.GameComponent
    {
        public int currentPoints;
        public int currentSize;
        public float timeElapsed;
        public Game1 game;

        Vector2 pointsPosition;
        Vector2 sizePosition;
        Vector2 timerPosition;

        public ScoreDisplay(Game1 game)
            : base(game)
        {
            currentPoints = 0;
            currentSize = 50;
            timeElapsed = 0;

            this.game = game;
            pointsPosition = new Vector2(20, 30);
            sizePosition = new Vector2(300, 30);
            timerPosition = new Vector2(game.screenWidth - 300, 30);
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
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            base.Update(gameTime);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void Draw(SpriteBatch batch)
        {

            batch.DrawString(Textures.font24, "Points: " + currentPoints.ToString(), pointsPosition, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            batch.DrawString(Textures.font24, "Size: " + currentSize.ToString(), sizePosition, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            batch.DrawString(Textures.font24, "Time: " + ((int)(timeElapsed/1000)).ToString(), timerPosition, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

        }
    }
}
