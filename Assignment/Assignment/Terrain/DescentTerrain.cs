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
        List<Rectangle> stepColliders;
        int heightStep;

        public DescentTerrain(Game1 game, float startAngle, double terrianHeight, bool hasFly)
            : base(game, startAngle, terrianHeight, hasFly)
        {
            texture = game.Content.Load<Texture2D>(@"Terrains/gr_2");
            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;
            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * scale), (int)(texture.Bounds.Height * scale));
            offsetAngle = 0.029f;

            // create list of smaller colliders that will act as steps:
            /*
             * ---------|
             * ------------|
             * ----------------|
             * ----------------------|
             */
            stepColliders = new List<Rectangle>();
            int widthStep = (int)(texture.Width * scale / 4);
            int heightOffset = (int)(texture.Height * scale / 4);
            heightStep = (int)(heightOffset / 4);
            for (int i = 0; i < 4; i++)
            {
                Rectangle step = new Rectangle(0, 0, widthStep * (i + 1), heightStep);
                stepColliders.Add(step);
            }
        }


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void adjustPosition(double tHeight)
        {
            position.X = (int)(game.earth.radius * 1f * (float)Math.Cos(angle) + game.screenWidth * 0.5f);
            position.Y = (int)(game.earth.radius * 1f * (float)Math.Sin(angle) + (game.screenHeight * GameSettings.TERRAIN_HEIGHT - tHeight+ game.earth.radius));

            // main collider
            boxCollider.X = (int)(position.X - 18f);
            boxCollider.Y = (int)(position.Y - 18f);

            // step colliders
            for (int i = 0; i < 4; i++)
            {
                Rectangle r = stepColliders[i];
                r.X = boxCollider.X - 15;
                r.Y = boxCollider.Y + heightStep * i;
                stepColliders[i] = r;
            }
            
            if (position.X < 0 - texture.Width || position.X > game.screenWidth + texture.Width)
            {
                isOnScreen = false;
            }
            else
            {
                isOnScreen = true;
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
            foreach (Rectangle r in stepColliders)
            {
                batch.Draw(t, r, c);
            }
            */
        }

        protected override void detectCollistions(GameTime gameTime)
        {
            if (boxCollider.Intersects(game.player.boxCollider))
            {
                foreach (Rectangle r in stepColliders)
                {
                    if (r.Intersects(game.player.boxCollider))
                        game.player.adjustPosition(r, gameTime);
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
    }
}
