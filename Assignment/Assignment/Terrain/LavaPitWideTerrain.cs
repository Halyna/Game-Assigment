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
    public class LavaPitWideTerrain : Terrain
    {

        Rectangle pitCollider;
        Rectangle leftRidgeCollider;
        Rectangle rightRidgeCollider;


        public LavaPitWideTerrain(Game1 game, float startAngle, bool hasFly)
            : base(game, startAngle, hasFly)
        {
            texture = game.Content.Load<Texture2D>(@"Terrains/lp_2");
            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;

            // colliders
            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * scale), (int)(texture.Bounds.Height * 0.9f * scale));
            /*
             * --|     |---
             * --|     |---
             * --|=====|---
             */
            pitCollider = new Rectangle(0, 0, (int)(boxCollider.Width * 0.7f), (int)(boxCollider.Height * 0.66f));
            leftRidgeCollider = new Rectangle(0, 0, (int)(boxCollider.Width * 0.15f), boxCollider.Height);
            rightRidgeCollider = new Rectangle(0, 0, (int)(boxCollider.Width * 0.15f), boxCollider.Height);

            offsetAngle = 0.055f;
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
            position.Y = (int)(game.earth.radius * 1f * (float)Math.Sin(angle) + (game.screenHeight * (GameSettings.TERRAIN_HEIGHT - 0.02) + game.earth.radius));

            // main collider
            boxCollider.X = (int)(position.X - 18f);
            boxCollider.Y = (int)(position.Y - 18f);

            // pit colliders
            pitCollider.X = (int)(boxCollider.X + leftRidgeCollider.Width);
            pitCollider.Y = (int)(boxCollider.Y + boxCollider.Height * 0.33f);
            leftRidgeCollider.X = boxCollider.X;
            leftRidgeCollider.Y = boxCollider.Y;
            rightRidgeCollider.X = (int)(boxCollider.X + boxCollider.Width - rightRidgeCollider.Width*1.5f);
            rightRidgeCollider.Y = boxCollider.Y;

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

        protected override void detectCollistions(GameTime gameTime)
        {
            if (boxCollider.Intersects(game.player.boxCollider))
            {
                if (rightRidgeCollider.Intersects(game.player.boxCollider))
                {
                    game.player.adjustPosition(rightRidgeCollider, gameTime);
                }
                else if (leftRidgeCollider.Intersects(game.player.boxCollider))
                {
                    game.player.adjustPosition(leftRidgeCollider, gameTime);
                }
                else if (pitCollider.Intersects(game.player.boxCollider))
                {
                    game.player.FallInPit(pitCollider);
                }
            }

            if (boxCollider.Intersects(game.bird.boxCollider))
            {
                game.bird.FlyAway();

            }
            if (boxCollider.Intersects(game.meteorSmall.boxCollider))
            {
                game.meteorSmall = new MeteorSmall(game);

            }
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.FlipHorizontally, 0f);
            if (fly != null)
            {
                fly.Draw(batch, gameTime);
            }

            /* debug: collider
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            Color c = new Color(0, 0, 0, 0.5f);
            batch.Draw(t, boxCollider, c);

            c = new Color(1, 1, 1, 0.5f);
            batch.Draw(t, pitCollider, c);
            batch.Draw(t, leftRidgeCollider, c);
            batch.Draw(t, rightRidgeCollider, c);
             */
        }

    }
}
