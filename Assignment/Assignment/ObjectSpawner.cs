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
    public class ObjectSpawner : Microsoft.Xna.Framework.GameComponent
    {
        public List<Bird> birds;
        public List<MeteorSmall> meteors;
        Game1 game;
        Random random;

        float timeTillBirdSpawn;
        float timeTillMeteorSpawn;


        public ObjectSpawner(Game1 game)
            : base(game)
        {
            this.game = game;
            random = new Random();
            Reset();
        }

        public override void Initialize()
        {       
            base.Initialize();
        }

        internal void Reset()
        {
            timeTillBirdSpawn = 0;
            timeTillMeteorSpawn = 0;
            birds = new List<Bird>();
            meteors = new List<MeteorSmall>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timeTillBirdSpawn -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timeTillMeteorSpawn -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeTillBirdSpawn <= 0)
            {
                // spawn new bird
                birds.Add(new Bird(game));
                // schedule next bird
                int stage = Math.Min(game.songPlaying, 5);
                timeTillBirdSpawn = random.Next(7 - stage, 10 - stage) * 1000.0f; // 6 to 8 sec for stage 1, 2 to 4 sec for stages 5-6
            }

            if (timeTillMeteorSpawn <= 0)
            {
                // spawn new meteor
                meteors.Add(new MeteorSmall(game));
                // schedule next meteor
                int stage = Math.Min(game.songPlaying, 5);
                timeTillMeteorSpawn = random.Next(7 - stage, 10 - stage) * 0.2f * 1000.0f; 
            }

            for (int i = birds.Count - 1; i >= 0; i-- )
                birds[i].Update(gameTime);

            for (int i = meteors.Count - 1; i >= 0; i--)
                meteors[i].Update(gameTime);
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            foreach (var bird in birds)
                bird.Draw(gameTime, batch);

            foreach (var meteor in meteors)
                meteor.Draw(batch, gameTime);
        }
    }
}
