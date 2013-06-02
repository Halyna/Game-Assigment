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
    public class Background : Microsoft.Xna.Framework.GameComponent
    {

        public List<BackgroundElement> back;
        public List<BackgroundElement> farBack;

        public Background(Game1 game)
            : base(game)
        {
            back = new List<BackgroundElement>();
            farBack = new List<BackgroundElement>();

            int statX = 0;
            Texture2D tex = game.Content.Load<Texture2D>(@"Backgrounds/bg_0");
            for (int i = 0; i < 4; i++)
            {
                BackgroundElement b = new BackgroundElement(game, tex, 0.3f, statX, 0.2f, 0.25f);
                this.farBack.Add(b);
                statX += (int)(tex.Bounds.Width * b.scale);
            }

            statX = 0;
            tex = game.Content.Load<Texture2D>(@"Backgrounds/bg_1");
            for (int i = 0; i < 4; i++)
            {
                BackgroundElement b = new BackgroundElement(game, tex, 0.3f, statX, 0.4f, 0.3f);
                this.farBack.Add(b);
                statX += (int)(tex.Bounds.Width * b.scale);

                //Console.WriteLine("Covered in terrains: {0} rad, left uncovered: {1}", angle - 4.4f, 2 * Math.PI - angle + 4.4f);
            }
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
           
            foreach (var bg in this.farBack)
            {
                bg.Update(gameTime);
            }


            base.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch batch)
        {
            foreach (var bg in this.farBack)
            {
                    bg.Draw(batch);
            }
        }
    }
}
