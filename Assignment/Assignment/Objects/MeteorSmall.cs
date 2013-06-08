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
    public class MeteorSmall : Microsoft.Xna.Framework.GameComponent
    {
        Game1 game;

        Texture2D texture;
        Vector2 position;
        float scale;
        public bool isAnimating = false;

        public Rectangle boxCollider;

        private Animation SmallMeteorAnimation;
        private AnimationController SmallMeteorAnimationController;

        public MeteorSmall(Game1 game)
            : base(game)
        {
            this.game = game;

            texture = game.Content.Load<Texture2D>(@"ObjectsAnimations/MeteorSmall/sm_0");
            Random r = new Random();
            position.X = r.Next(game.screenWidth);
            position.Y = -10;

            scale = 0.13f;

            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * scale * 0.6f), (int)(texture.Bounds.Height * scale * 0.6f));

            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            SmallMeteorAnimation = Textures.MeteorSmallAnimation;
            SmallMeteorAnimation.CurrentTexture = 0;
            SmallMeteorAnimationController.PlayAnimation(SmallMeteorAnimation);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (isAnimating)
            {
                SmallMeteorAnimationController.Update(gameTime);
                if (SmallMeteorAnimationController.Finished)
                {
                    game.objectSpawner.meteors.Remove(this);
                }
            }

            else
            {

                position += new Vector2(-6f, 6f);
                this.boxCollider.X = (int)position.X + (int)((texture.Bounds.Width * scale - boxCollider.Width) / 2);
                this.boxCollider.Y = (int)position.Y + (int)((texture.Bounds.Height * scale - boxCollider.Height) / 2);

                detectCollistions();
            }
        }

        private void detectCollistions()
        {

            if (boxCollider.Intersects(game.player.boxCollider) && game.player.isCrouching != true)
            {
                game.hitTerrainSound.Play();
                game.player.MeteorCollided(this);
            }
        }

        public virtual void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (isAnimating)
                SmallMeteorAnimationController.Draw(gameTime, batch, position, scale, SpriteEffects.None, Color.DarkOliveGreen, 0, Vector2.Zero);

            else
                batch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);

            /* debug: collider
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            Color c = new Color(0, 0, 0, 0.5f);
            batch.Draw(t, boxCollider, c);
            */
        }

        internal void Animate()
        {
            isAnimating = true;
        }
    }
}
