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
    public class DescentTerrain : Terrain
    {
        public DescentTerrain(Game1 game, float startAngle, bool hasFly)
            : base(game, startAngle, hasFly)
        {
            texture = game.Content.Load<Texture2D>(@"Terrains/gr_2");
            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;
            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * scale), (int)(texture.Bounds.Height * scale));
            offsetAngle = 0.029f;
        }


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void adjustPosition()
        {
            position.X = (int)(game.earth.radius * 1f * (float)Math.Cos(angle) + game.screenWidth * 0.5f);
            position.Y = (int)(game.earth.radius * 1f * (float)Math.Sin(angle) + (game.screenHeight * 0.62f + game.earth.radius));
            boxCollider.X = (int)(position.X - 18f);//(int)position.X - boxCollider.Width;
            boxCollider.Y = (int)(position.Y - 18f); //+ boxCollider.Height/3;
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

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.FlipHorizontally, 0f);
            if (fly != null)
            {
                fly.Draw(batch);
            }

            // debug: collider
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            Color c = new Color(0, 0, 0, 0.5f);
            //batch.Draw(t, boxCollider, c);
        }
    }
}
