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
    public class MeteorBig : Microsoft.Xna.Framework.GameComponent
    {
        Texture2D texture;
        Vector2 position;
        Vector2 origin;
        float rotationAngle;
        float scale;

        private Animation BigMeteorAnimation;
        private AnimationController BigMeteorAnimationController;

        public MeteorBig(Game1 game)
            : base(game)
        {
            texture = game.Content.Load<Texture2D>(@"ObjectsAnimations/MeteorBig/bm_0");
            position.X = game.screenWidth - 50;
            position.Y = 10;

            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;

            rotationAngle = 0.9f;
            scale = 0.05f;

            Initialize();
        }

     
        public override void Initialize()
        {
            base.Initialize();
            BigMeteorAnimation = Textures.MeteorBigAnimation;
            BigMeteorAnimationController.PlayAnimation(BigMeteorAnimation);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            BigMeteorAnimationController.Update(gameTime);

            scale += 0.0001f;
            position += new Vector2(0.003f, -0.0005f);

            // need stopping conditions && link to time elapsed
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            BigMeteorAnimationController.Draw(gameTime, batch, position, scale, SpriteEffects.None, Color.White, rotationAngle, origin);
           

        }
    }
}
