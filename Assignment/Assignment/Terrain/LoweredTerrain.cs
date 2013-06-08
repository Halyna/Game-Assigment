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
    public class LoweredTerrain : Terrain
    {
        public LoweredTerrain(Game1 game, float startAngle, double terrianHeight, bool hasFly)
            : base(game, startAngle, terrianHeight, hasFly)
        {
            texture = game.Content.Load<Texture2D>(@"Terrains/gr_1");
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

        public override void adjustPosition(double tHeight)
        {
            position.X = (int)(game.earth.radius * 1f * (float)Math.Cos(angle) + game.screenWidth * 0.5f) ;
            position.Y = (int)(game.earth.radius * 1f * (float)Math.Sin(angle) + (game.screenHeight * (GameSettings.TERRAIN_HEIGHT - tHeight) + game.earth.radius));// +boxCollider.Height * 0.5f;
            boxCollider.X = (int)(position.X - 17f);//(int)position.X - boxCollider.Width;
            boxCollider.Y = (int)(position.Y - 9f); //+ boxCollider.Height/3;
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
    }
}
