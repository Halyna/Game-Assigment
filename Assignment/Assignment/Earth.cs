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
        double lowestH;
        double secondH;
        double thirdH;
        double topH;

        public List<Terrain> terrains;

        public Earth(Game1 game)
            : base(game)
        {
            this.game = game;
            this.position = new Vector2(game.screenWidth * 0.5f, game.screenHeight * 7.05f);
            texture = game.Content.Load<Texture2D>(@"Earth");
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
            radius = texture.Bounds.Height * 0.5f * scale;
            lowestH = 0.01;
            secondH = 0.1;
            thirdH = 0.2;
            topH = 0.3;




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
                Random rand = new Random();
                for (int i = 0; i < 15; i++)
                {
                    
                    Terrain t = new PlainTerrain(game, angle, lowestH, true);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;
                    Terrain lastTerrain = t;

                    t = generateRandomTerrain(lastTerrain, angle, rand);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;
                    lastTerrain = t;

                    t = generateRandomTerrain(lastTerrain, angle, rand);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;
                    lastTerrain = t;

                    t = generateRandomTerrain(lastTerrain, angle, rand);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;
                    lastTerrain = t;

                    t = generateRandomTerrain(lastTerrain, angle, rand);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;
                    lastTerrain = t;

                    t = generateRandomTerrain(lastTerrain, angle, rand);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;
                    lastTerrain = t;

                    t = generateRandomTerrain(lastTerrain, angle, rand);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;
                    lastTerrain = t;

                    t = generateRandomTerrain(lastTerrain, angle, rand);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;
                    lastTerrain = t;

                    t = generateRandomTerrain(lastTerrain, angle, rand);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;
                    lastTerrain = t;

                    t = generateRandomTerrain(lastTerrain, angle, rand);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;
                    lastTerrain = t;

                    t = generateRandomTerrain(lastTerrain, angle, rand);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    Console.WriteLine("Covered in terrains: {0} rad, left uncovered: {1}", angle - 4.4f, 2 * Math.PI - angle + 4.4f); 
                }
            }
        }

        public Terrain generateRandomTerrain(Terrain lastTerrain, float angle, Random rand)
        {
            lastTerrain = this.terrains[terrains.Count -1];

            bool fly = false;
            int flyChance = rand.Next(0, 10);
            if (flyChance == 9){
                fly = true;
            }

            double terrainHeight = lowestH;
            int terNum = rand.Next(0,4);
            

            if (terNum == 1)
            {
                terrainHeight = secondH;
            }
            if (terNum == 2 && lastTerrain.tHeight != lowestH)
            {
                terrainHeight = thirdH;
            }
            if (terNum == 3 && lastTerrain.tHeight != lowestH && lastTerrain.tHeight != secondH)
            {
                terrainHeight = topH;
            }
            if (lastTerrain is AscentTerrain)
            {
                if (lastTerrain.tHeight == lowestH)
                {
                    terrainHeight = lowestH;
                }
                if (lastTerrain.tHeight == secondH)
                {
                    terrainHeight = secondH;
                }
                if (lastTerrain.tHeight == thirdH)
                {
                    terrainHeight = thirdH;
                }
            }
         
            Terrain[] allTerrains = new Terrain[7];
            allTerrains[0] = new PlainTerrain(game, angle, terrainHeight, fly);
            allTerrains[1] = new AscentTerrain(game, angle, terrainHeight, fly);
            allTerrains[2] = new VolcanoTerrain(game, angle, lastTerrain.tHeight, fly);
            allTerrains[3] = new LavaPitTerrain(game, angle, terrainHeight, fly);
            allTerrains[4] = new LavaPitWideTerrain(game, angle, terrainHeight, fly);
            allTerrains[5] = new LoweredTerrain(game, angle, terrainHeight, fly);
            allTerrains[6] = new DescentTerrain(game, angle, terrainHeight, fly);


            Terrain nextTerrain = allTerrains[rand.Next(0, 6)];
            return nextTerrain;
            
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
