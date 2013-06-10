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
    
    public class Fly : Microsoft.Xna.Framework.GameComponent
    {

        Texture2D texture;
        Game1 game;
        public Terrain terrain;

        Vector2 position;
        Vector2 origin;
        float scale;
        float rotationAngle; // radians
        float positionAngle; // radians
        public Rectangle boxCollider;

        private Animation FlyAnimation;
        private AnimationController FlyAnimationController;


        public Fly(Game1 game, float startAngle, Terrain terrain)
            : base(game)
        {
            this.game = game;
            this.terrain = terrain;

            this.position = new Vector2();
            Random r = new Random();
            scale = 0.5f;
            //position.X = r.Next(game.screenWidth);
            position.Y = game.screenHeight * 0.5f;
            positionAngle = startAngle;
            texture = game.Content.Load<Texture2D>(@"ObjectsAnimations/Fly/fl_0");
            origin.X = texture.Width * 2 * scale;
            origin.Y = terrain.position.Y - 100;
            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * scale), (int)(texture.Bounds.Height * scale));
            Initialize();
        }

       
        public override void Initialize()
        {
            base.Initialize();
            FlyAnimation = Textures.FlyAnimation;
            FlyAnimationController.PlayAnimation(FlyAnimation);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            FlyAnimationController.Update(gameTime);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            positionAngle -= elapsed * GameSettings.EARTH_ROTATION_SPEED;

            rotationAngle += 0.1f;
            origin.X = (int)(game.earth.radius * 1f * (float)Math.Cos(positionAngle) + game.screenWidth * 0.5f);
            
            if (terrain.GetType() == typeof(VolcanoTerrain))
                origin.Y = terrain.position.Y;// volcanos have large offset
            else
                origin.Y = terrain.position.Y - game.player.boxCollider.Height * 2f;
            position = origin + Vector2.Transform(new Vector2(20, 0), Matrix.CreateRotationZ(rotationAngle));

            this.boxCollider.X = (int)position.X;
            this.boxCollider.Y = (int)position.Y;

            detectCollistions();
        }

        private void detectCollistions()
        {
            if (boxCollider.Intersects(game.player.boxCollider))
            {
                game.biteSound.Play();
                game.player.flyCollided(this);

            }
        }
        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            FlyAnimationController.Draw(gameTime, batch, position, scale, SpriteEffects.None, Color.DarkOliveGreen, 0, Vector2.Zero);
            //batch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);

            /* debug - box collider
            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            Color c = new Color(0, 0, 0, 0.5f);
            batch.Draw(t, boxCollider, c);
            */

        }
    }
}
