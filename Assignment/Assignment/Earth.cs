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
    public class Earth : Microsoft.Xna.Framework.GameComponent
    {
 
        Texture2D texture;
        Game1 game;
        Vector2 position;
        public Vector2 origin;
        float rotationAngle;
        public float scale = 6f;
        public float radius;

        public List<Terrain> terrains;

        public Earth(Game1 game)
            : base(game)
        {
            this.game = game;
            this.position = new Vector2(game.screenWidth * 0.5f, game.screenHeight * 7f);
            texture = game.Content.Load<Texture2D>(@"Earth");
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
            radius = texture.Bounds.Height * 0.5f * scale;

            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
           
            base.Initialize();

            if (this.terrains == null)
            {
                  this.terrains = new List<Terrain>();
                float angle = 4.4f;
                for (int i = 0; i < 15; i++)
                {
                    Terrain t = new PlainTerrain(game, angle, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    t = new LavaPitTerrain(game, angle, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    t = new PlainTerrain(game, angle, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    t = new LavaPitWideTerrain(game, angle, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    t = new DescentTerrain(game, angle, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    t = new LoweredTerrain(game, angle, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    t = new LoweredTerrain(game, angle, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    t = new VolcanoTerrain(game, angle, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    t = new LoweredTerrain(game, angle, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    t = new LoweredTerrain(game, angle, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    t = new AscentTerrain(game, angle, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    Console.WriteLine("Covered in terrains: {0} rad, left uncovered: {1}", angle - 4.4f, 2 * Math.PI - angle + 4.4f); 
                }
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your game logic here.
            rotationAngle -= elapsed * GameSettings.EARTH_ROTATION_SPEED;
            float circle = MathHelper.Pi * 2;
            rotationAngle = rotationAngle % circle;


            foreach (var terrain in this.terrains)
            {
                    terrain.Update(gameTime);
            }
           

            base.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch batch, GameTime gameTime) 
        {
           batch.Draw(texture, position, null, Color.White, rotationAngle, origin, scale, SpriteEffects.None, 0f);
           foreach (var terrain in this.terrains)
           {

               if (terrain.isOnScreen)
               {
                   terrain.Draw(batch, gameTime);
               }
           }
        }
    }
}
