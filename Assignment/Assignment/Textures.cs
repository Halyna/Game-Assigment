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

            // menus
            gameOverText = Content.Load<Texture2D>("UI/gameovertext");
            startGameText = Content.Load<Texture2D>("UI/gameovertext");
            logoText = Content.Load<Texture2D>("UI/logoText");

            // fonts
            font24 = Content.Load<SpriteFont>("Fonts/Font24");

        }
    }
}
