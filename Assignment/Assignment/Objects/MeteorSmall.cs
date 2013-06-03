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
        int direction;

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

            if (position.X > game.screenWidth * 0.5f)
                direction = -1;
            else
                direction = 1;
            scale = 0.1f;

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

            SmallMeteorAnimationController.Update(gameTime);

            position += new Vector2(- 6f, 6f);
            this.boxCollider.X = (int)position.X;
            this.boxCollider.Y = (int)position.Y;


            detectCollistions();
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
            SmallMeteorAnimationController.Draw(gameTime, batch, position, scale, SpriteEffects.None, Color.DarkOliveGreen, 0, Vector2.Zero);


        }
    }
}
