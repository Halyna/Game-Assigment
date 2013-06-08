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
    public class VolcanoTerrain : Terrain
    {
        const float inactiveTime = 2000;
        private Animation volcanoAnimation;
        private AnimationController VolcanoAnimationController;

        bool isAnimating;
        float timeTillAnimation = inactiveTime;

        List<Rectangle> stepCollidersAscend;
        List<Rectangle> stepCollidersDescend;
        int heightStep;
        int widthStep;

        Rectangle lavaCollider;

        public VolcanoTerrain(Game1 game, float startAngle, double terrianHeight, bool hasFly)
            : base(game, startAngle, terrianHeight, hasFly)
        {
            texture = game.Content.Load<Texture2D>(@"Terrains/vo_0");
            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;

            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * scale), (int)(texture.Bounds.Height * scale));

            // create list of smaller colliders that will act as steps and lava:
            /*
             *                         |---|
             *                         |---| 
             *                -------| |---| |------ 
             *           ------------|       |-------------  
             *       ----------------|       |-----------------
             * ----------------------|       |------------------------
             */
            stepCollidersAscend = new List<Rectangle>();
            widthStep = (int)(texture.Width * scale / 10);
            int heightOffset = (int)(texture.Height * scale / 4);
            heightStep = (int)(heightOffset / 4);
            for (int i = 0; i < 4; i++)
            {
                Rectangle step = new Rectangle(0, 0, widthStep * (i + 1), heightStep);
                stepCollidersAscend.Add(step);
            }

            stepCollidersDescend = new List<Rectangle>();
            for (int i = 0; i < 4; i++)
            {
                Rectangle step = new Rectangle(0, 0, widthStep * (i + 1), heightStep);
                stepCollidersDescend.Add(step);
            }

            lavaCollider = new Rectangle(0, 0, (int)(boxCollider.Width * 0.02), (int)(texture.Bounds.Height * scale));

            offsetAngle = 0.055f;

            volcanoAnimation = Textures.VolcanoAnimation;
            VolcanoAnimationController.PlayAnimation(volcanoAnimation);
        }


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!isOnScreen)
                return;

            //if (isAnimating)
            //{
                VolcanoAnimationController.Update(gameTime);
            //    if (VolcanoAnimationController.Finished)
            //    {
            //        timeTillAnimation = inactiveTime;
            //        Console.WriteLine("timeTillAnimation " + timeTillAnimation);
            //        isAnimating = false;
            //        VolcanoAnimationController.Animation.CurrentTexture = 0;
            //    }
            //}

            //else
            //{
            //    timeTillAnimation -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //    if (timeTillAnimation < 0)
            //        isAnimating = true;
            //}
        }

        public override void adjustPosition(double tHeight)
        {
            position.X = (int)(game.earth.radius * 1f * (float)Math.Cos(angle) + game.screenWidth * 0.5f);
            position.Y = (int)(game.earth.radius * 1f * (float)Math.Sin(angle) + (game.screenHeight * (GameSettings.TERRAIN_HEIGHT -tHeight - 0.22) + game.earth.radius));

            
             // main collider
            boxCollider.X = (int)(position.X - 18f);
            boxCollider.Y = (int)(position.Y + boxCollider.Height * 0.25f);

            // lava collider
            lavaCollider.X = (int)(boxCollider.X + ((boxCollider.Width - lavaCollider.Width)/2) - 15);
            lavaCollider.Y = (int)(position.Y + boxCollider.Height * 0.25f);

            // step colliders
            for (int i = 0; i < 4; i++)
            {
                Rectangle r = stepCollidersAscend[i];
                r.X = boxCollider.X + widthStep * (3 - i);
                r.Y = boxCollider.Y + heightStep * i;
                stepCollidersAscend[i] = r;
            }
            for (int i = 0; i < 4; i++)
            {
                Rectangle r = stepCollidersDescend[i];
                r.X = boxCollider.X + (int)(boxCollider.Width - widthStep * 5f);
                r.Y = boxCollider.Y + heightStep * i;
                stepCollidersDescend[i] = r;
            }

            if (position.X < 0 - texture.Width || position.X > game.screenWidth + texture.Width)
                isOnScreen = false;
            else
                isOnScreen = true;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (!isOnScreen)
                return;

            //if (isAnimating)
                VolcanoAnimationController.Draw(gameTime, batch, position, scale, SpriteEffects.None, Color.White, 0, origin);
           // else
            //    batch.Draw(texture, position, null, Color.Black, 0, origin, scale, SpriteEffects.None, 0f);

            if (fly != null)
            {
                fly.Draw(batch, gameTime);
            }

            /* debug: collider * */
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            Color c = new Color(0, 0, 0, 0.5f);
            batch.Draw(t, boxCollider, c);

            c = new Color(1, 1, 1, 0.5f);
            foreach (Rectangle r in stepCollidersAscend)
            {
                batch.Draw(t, r, c);
            }
            foreach (Rectangle r in stepCollidersDescend)
            {
               batch.Draw(t, r, c);
            }
            batch.Draw(t, lavaCollider, c);
            
        }

        protected override void detectCollistions(GameTime gameTime)
        {
            if (boxCollider.Intersects(game.player.boxCollider))
            {
                foreach (Rectangle r in stepCollidersAscend)
                {
                    if (r.Intersects(game.player.boxCollider))
                    {
                        game.player.adjustPosition(r, gameTime);
                        break;
                    }
                }

                foreach (Rectangle r in stepCollidersDescend)
                {
                    if (r.Intersects(game.player.boxCollider))
                    {
                        game.player.adjustPosition(r, gameTime);
                        break;
                    }
                }

                if (lavaCollider.Intersects(game.player.boxCollider))
                    game.player.FallInPit(lavaCollider);
            }

            base.detectCollistions(gameTime);

        }
    }
}
