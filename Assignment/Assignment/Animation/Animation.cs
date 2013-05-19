#region File Description
//-----------------------------------------------------------------------------
// Animation.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework.Graphics;

namespace Assignment
{
    public class Animation
    {
        public Texture2D Texture
        {
            get { return textures[CurrentTexture]; }
        }
        Texture2D[] textures;


        public int CurrentTexture { get; set; }
        public int MaxTextures { get { return textures.Length; } }

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = value; }
        }
        float frameTime;

        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;

        public int FrameCount
        {
            get { return frameCounts[CurrentTexture]; }
        }
        int[] frameCounts;

        public int FrameWidth
        {
            // Assume square frames.
            get { return Texture.Width / frameCounts[CurrentTexture]; }
        }

        public int FrameHeight
        {
            get { return Texture.Height; }
        }
      
        public Animation(Texture2D texture, float frameTime, bool isLooping, int frameCount)
        {
            this.textures = new Texture2D[1];
            textures[0] = texture;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
            this.frameCounts = new int[1];
            this.frameCounts[0] = frameCount;
        }

        public Animation(Texture2D[] texture, float frameTime, bool isLooping, int[] frameCount) {
            this.textures = texture;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
            this.frameCounts = frameCount;
        }
    }
}