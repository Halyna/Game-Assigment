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
            this.position = new Vector2(game.screenWidth * 0.5f, game.screenHeight * 7f);
            // earth center
            texture = game.Content.Load<Texture2D>(@"Earth");
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
            radius = texture.Bounds.Height * 0.5f * scale;
            lowestH = 0.01;
            secondH = 0.08;
            thirdH = 0.15;
            topH = 0.2;

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
                float uncoveredSurface = 2 * (float)Math.PI; // full circle

                // fist piece
                Terrain t = new PlainTerrain(game, angle, lowestH, true);
                this.terrains.Add(t);
                angle += t.offsetAngle;
                uncoveredSurface -= t.offsetAngle;

                // rest generated randomly to cover full circle
                while (uncoveredSurface > 0)
                {
                    t = generateRandomTerrain(angle, rand);
                    this.terrains.Add(t);
                    angle += t.offsetAngle;

                    uncoveredSurface -= t.offsetAngle;
                }

                Console.WriteLine("Covered in terrains: {0} rad, left uncovered: {1}", angle - 4.4f, 2 * Math.PI - angle + 4.4f);
            }
        }

        public Terrain generateRandomTerrain(float angle, Random rand)
        {
            Terrain lastTerrain = this.terrains[terrains.Count - 1];

            bool fly = false;
            int flyChance = rand.Next(0, 4);
            if (flyChance == 0)
            {
                fly = true;
            }

            double terrainHeight = lowestH;
            int terNum = rand.Next(0, 4);

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

            Terrain[] allTerrains = new Terrain[7];
            allTerrains[0] = new PlainTerrain(game, angle, terrainHeight, fly);
            allTerrains[1] = new AscentTerrain(game, angle, terrainHeight, fly);
            allTerrains[2] = new LavaPitTerrain(game, angle, terrainHeight, fly);
            allTerrains[3] = new LavaPitWideTerrain(game, angle, terrainHeight, fly);
            allTerrains[4] = new LoweredTerrain(game, angle, terrainHeight, fly);
            allTerrains[5] = new DescentTerrain(game, angle, terrainHeight, fly);

            // volcanos get too high..
            double volcanoHeight = lastTerrain.tHeight == topH || lastTerrain.tHeight == thirdH ? secondH : lastTerrain.tHeight;
            Console.WriteLine("volcano height " + volcanoHeight);
            allTerrains[6] = new VolcanoTerrain(game, angle, volcanoHeight, fly);

            Terrain nextTerrain = allTerrains[rand.Next(0, 7)];
            if (lastTerrain is AscentTerrain)
            {
                if (lastTerrain.tHeight == lowestH)
                {
                    nextTerrain.tHeight = lowestH;
                }
                if (lastTerrain.tHeight == secondH)
                {
                    nextTerrain.tHeight = secondH;
                }
                if (lastTerrain.tHeight == thirdH)
                {
                    nextTerrain.tHeight = thirdH;
                }
                if (nextTerrain is AscentTerrain)
                {
                    nextTerrain = generateRandomTerrain(angle, rand);
                }
            }

            if (lastTerrain is DescentTerrain)
            {
                if (lastTerrain.tHeight == lowestH)
                {
                    nextTerrain.tHeight = lowestH;
                }
                if (lastTerrain.tHeight == secondH)
                {
                    nextTerrain.tHeight = secondH;
                }
                if (lastTerrain.tHeight == thirdH)
                {
                    nextTerrain.tHeight = thirdH;
                }
                if (nextTerrain is DescentTerrain)
                {
                    nextTerrain = generateRandomTerrain(angle, rand);
                }
            }

            return nextTerrain;

        }


        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // no need to rotate - sprite all black now..
            // rotationAngle -= elapsed * GameSettings.EARTH_ROTATION_SPEED;
            //float circle = MathHelper.Pi * 2;
            //rotationAngle = rotationAngle % circle;


            foreach (var terrain in this.terrains)
            {
                terrain.Update(gameTime);
            }

            base.Update(gameTime);
        }


        public virtual void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0f);
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
