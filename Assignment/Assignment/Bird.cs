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
    public class Bird : Microsoft.Xna.Framework.GameComponent
    {

        Texture2D texture;
        private Animation BirdAnimation;
        private AnimationPlayer BirdAnimationController;



        Game1 game;
        Vector2 position;
        Vector2 target;
        Vector2 origin;
        float scale;
        public Rectangle boxCollider;

        public Bird(Game1 game, Player player)
            : base(game)
        {
            this.game = game;
           
            this.position = new Vector2();
            Random r = new Random();
            scale = 0.3f;
            position.X = r.Next(game.screenWidth);
            position.Y = 0;
            target = new Vector2();
            texture = game.Content.Load<Texture2D>("ObjectsAnimations/Bird/d_FLAP_0");
            origin.X = texture.Width / 2 * scale;
            origin.Y = texture.Height / 2 * scale;
            boxCollider = new Rectangle(0, 0, (int)(texture.Bounds.Width * 0.8f * scale), (int)(texture.Bounds.Height * 0.6f * scale));
            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            game.birdSpawnedSound.Play();
            BirdAnimation = Textures.BirdAnimation;
            BirdAnimationController.PlayAnimation(BirdAnimation);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            BirdAnimationController.Update(gameTime);

           // will stop adjusting direction at some point
            if (position.Y < game.screenHeight / 3)
            {
                // Direction between two poins is Vector(x2-x1,y2-y1)
                target = new Vector2(game.player.position.X - position.X, game.player.position.Y - position.Y);
                target.Normalize();
            }
           

            position += target * 4;
            this.boxCollider.X = (int)position.X;
            this.boxCollider.Y = (int)position.Y;
           

            detectCollistions();
            
        }

        private void detectCollistions()
        {
            
            if (boxCollider.Intersects(game.player.boxCollider) && game.player.isCrouching != true)
            {

                game.hitTerrainSound.Play();
                game.player.birdCollided(this);


            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {
           // batch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0f);
            if (position.X > game.player.position.X)
            {
                BirdAnimationController.Draw(gameTime, batch, position, scale, SpriteEffects.FlipHorizontally, Color.DarkOliveGreen, 0, origin);
            }
            else
            {
                BirdAnimationController.Draw(gameTime, batch, position, scale, SpriteEffects.None, Color.DarkOliveGreen, 0, origin);
            }
           

            var t = new Texture2D(game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            Color c = new Color(0, 0, 0, 0.5f);
            batch.Draw(t, boxCollider, c);


        }
    }
}
