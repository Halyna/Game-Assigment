using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment
{
    class Textures
    {
        private static ContentManager Content;

        // player
        public static Animation PlayerIdleAnimation;
        public static Animation PlayerMoveingAnimation;
        public static Animation PlayerJumpingAnimation;
        public static Animation PlayerCrouchAnimation;

        public static Animation BirdAnimation;

        public static Animation VolcanoAnimation;

        public static Animation FlyAnimation;

        public static Animation MeteorBigAnimation;
        public static Animation MeteorSmallAnimation;
        
        // menus
        public static Texture2D gameOverText;
        public static Texture2D startGameText;
        public static Texture2D logoText;

        // fonts
        public static SpriteFont font24;

        public static void Initialize(ContentManager content)
        {
            Content = content;
            LoadTextures();
        }

        public  static void LoadTextures()
        {
            // Idle
            Texture2D[] playerIdleArray = new Texture2D[6];
            int[] playerIdleFrames = new int[6] { 1, 1, 1, 1, 1, 1 };
            String frameName;
            for (int i = 0; i < playerIdleArray.Length; i++)
            {
                frameName = String.Format("PlayerAnimations/Idle/t_IDLE_{0}", i);
                playerIdleArray[i] = Content.Load<Texture2D>(frameName);
            }

            PlayerIdleAnimation = new Animation(playerIdleArray, .1f, true, playerIdleFrames);

            // Moving
            Texture2D[] playerMovingArray = new Texture2D[5];
            int[] playerMovingFrames = new int[5] { 1, 1, 1, 1, 1 };
            for (int i = 0; i < playerMovingFrames.Length; i++)
            {
                frameName = String.Format("PlayerAnimations/Run/t_RUN_{0}", i);
                playerMovingArray[i] = Content.Load<Texture2D>(frameName);
            }

            PlayerMoveingAnimation = new Animation(playerMovingArray, .1f, true, playerMovingFrames);

            // Jumping
            Texture2D[] playerJumpingArray = new Texture2D[5];
            int[] playerJumpingFrames = new int[5] { 1, 1, 1, 1, 1 };
            for (int i = 0; i < playerJumpingArray.Length; i++)
            {
                frameName = String.Format("PlayerAnimations/Jump/t_JUMP_{0}", i);
                playerJumpingArray[i] = Content.Load<Texture2D>(frameName);
            }

            PlayerJumpingAnimation = new Animation(playerJumpingArray, .1f, true, playerJumpingFrames);

            // Crouching
            Texture2D[] playerCrouchArray = new Texture2D[4];
            int[] playerCrouchFrames = new int[4] { 1, 1, 1, 1 };
            for (int i = 0; i < playerCrouchArray.Length; i++)
            {
                frameName = String.Format("PlayerAnimations/Crouch/t_CROUCH_{0}", i);
                playerCrouchArray[i] = Content.Load<Texture2D>(frameName);
            }

            PlayerCrouchAnimation = new Animation(playerCrouchArray, .1f, false, playerCrouchFrames);

            // Bird
            Texture2D[] birdArray = new Texture2D[8];
            int[] birdFrames = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
            for (int i = 0; i < birdArray.Length; i++)
            {
                frameName = String.Format("ObjectsAnimations/Bird/d_FLAP_{0}", i);
                birdArray[i] = Content.Load<Texture2D>(frameName);
            }

            BirdAnimation = new Animation(birdArray, .1f, true, birdFrames);

            // Volcano
            Texture2D[] volcanoArray = new Texture2D[12];
            int[] volcanoFrames = new int[12] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            for (int i = 0; i < volcanoArray.Length; i++)
            {
                frameName = String.Format("Terrains/vo_{0}", i);
                volcanoArray[i] = Content.Load<Texture2D>(frameName);
            }

            VolcanoAnimation = new Animation(volcanoArray, .5f, true, volcanoFrames);

            // Fly
            Texture2D[] flyArray = new Texture2D[2];
            int[] flyFrames = new int[2] { 1, 1 };
            for (int i = 0; i < flyArray.Length; i++)
            {
                frameName = String.Format("ObjectsAnimations/Fly/fl_{0}", i);
                flyArray[i] = Content.Load<Texture2D>(frameName);
            }

            FlyAnimation = new Animation(flyArray, .2f, true, flyFrames);

            // Meteors
            Texture2D[] metBigArray = new Texture2D[2];
            int[] metBigFrames = new int[2] { 1, 1 };
            for (int i = 0; i < metBigArray.Length; i++)
            {
                frameName = String.Format("ObjectsAnimations/MeteorBig/bm_{0}", i);
                metBigArray[i] = Content.Load<Texture2D>(frameName);
            }

            MeteorBigAnimation = new Animation(metBigArray, .2f, true, metBigFrames);

            Texture2D[] metSmallArray = new Texture2D[2];
            int[] metSmallFrames = new int[2] { 1, 1 };
            for (int i = 0; i < metSmallArray.Length; i++)
            {
                frameName = String.Format("ObjectsAnimations/MeteorSmall/sm_{0}", i);
                metSmallArray[i] = Content.Load<Texture2D>(frameName);
            }

            MeteorSmallAnimation = new Animation(metSmallArray, 1f, false, metSmallFrames);

            // menus
            gameOverText = Content.Load<Texture2D>("UI/gameovertext");
            startGameText = Content.Load<Texture2D>("UI/gameovertext");
            logoText = Content.Load<Texture2D>("UI/logoText");

            // fonts
            font24 = Content.Load<SpriteFont>("Fonts/Font24");

        }
    }
}
