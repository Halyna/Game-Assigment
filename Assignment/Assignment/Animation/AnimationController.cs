#region File Description
//-----------------------------------------------------------------------------
// AnimationPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assignment
{

    struct AnimationController
    {
       
        public Animation Animation
        {
            get { return animation; }
        }
        Animation animation;

        public int FrameIndex
        {
            get { return frameIndex; }
        }
        int frameIndex;

       
        private float time;

        private Rectangle source;

      
        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight); }
        }

        public bool Finished { get; set; }

        public void PlayAnimation(Animation animation)
        {
            if (Animation == animation)
                return;
            
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
            source = new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.Texture.Height);
        }

        public void Update(GameTime gameTime)
        {
            if (Animation == null)
                throw new NotSupportedException("No animation is currently playing.");

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (time > Animation.FrameTime)
            {
                time -= Animation.FrameTime;
                if (Animation.IsLooping) {
                    if (frameIndex + 1 > Animation.FrameCount - 1) {
                        if (animation.CurrentTexture >= animation.MaxTextures - 1) {
                            frameIndex = 0;
                            animation.CurrentTexture = 0;
                        } else {
                            frameIndex = 0;
                            animation.CurrentTexture++;
                        }
                    } else {
                        frameIndex++;
                    }
                    //frameIndex = (frameIndex + 1) % Animation.FrameCount;
                } else {
                    if (frameIndex + 1 > Animation.FrameCount - 1) {
                        if (animation.CurrentTexture >= animation.MaxTextures - 1) {
                            Finished = true;
                        } else {
                            frameIndex = 0;
                            animation.CurrentTexture++;
                        }
                    } else {
                        frameIndex = Math.Min(frameIndex + 1, Animation.FrameCount - 1);
                    }
                }
            }

            source = new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.Texture.Height);            
        }

       

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, Color color)
        {
            spriteBatch.Draw(Animation.Texture, position, source, color, 0.0f, new Vector2(animation.FrameWidth / 2, animation.FrameHeight / 2), 1.0f, spriteEffects, 0.0f);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float scale, SpriteEffects spriteEffects, Color color, float rotation)
        {  
            spriteBatch.Draw(Animation.Texture, position, source, color, rotation, new Vector2(animation.FrameWidth / 2, animation.FrameHeight / 2), scale, spriteEffects, 0.0f);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float scale, SpriteEffects spriteEffects, Color color, float rotation, Vector2 origin) {
            spriteBatch.Draw(Animation.Texture, position, source, color, rotation, origin, scale, spriteEffects, 0.0f);
        }
    }
}
