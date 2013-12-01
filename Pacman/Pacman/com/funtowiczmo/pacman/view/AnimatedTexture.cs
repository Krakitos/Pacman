#region File Description
//-----------------------------------------------------------------------------
// AnimatedTexture.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.com.funtowiczmo.pacman.entity;

namespace Pacman.com.funtowiczmo.pacman.utils
{
    public class AnimatedTexture
    {
        private float timePerFrame;
        private float totalElapsed;
        private int frame;
        public float scale;
        private bool paused;
        private List<Texture2D> skins;

        public AnimatedTexture()
        {
            this.scale = 1;
        }

        public AnimatedTexture(float scale)
        {
            this.scale = scale;
        }

        protected void Load(string asset, int framesPerSec)
        {
            skins = new List<Texture2D>();

            skins.Add(AssetsManager.GetInstance().GetTexture(asset));
            timePerFrame = (float)1 / framesPerSec;
            frame = 0;
            totalElapsed = 0;
            paused = false;
        }

        protected void Load(string[] assets, int framesPerSec)
        {
            skins = new List<Texture2D>(assets.Length);

            for (int i = 0; i < assets.Length; i++) skins.Add(AssetsManager.GetInstance().GetTexture(assets[i]));

            timePerFrame = (float)1 / framesPerSec;
            frame = 0;
            totalElapsed = 0;
            paused = false;
        }

        protected void Load(string[] assets)
        {
            skins = new List<Texture2D>(assets.Length);

            for (int i = 0; i < assets.Length; i++) skins.Add(AssetsManager.GetInstance().GetTexture(assets[i]));

            timePerFrame = (float)1/assets.Length;
            frame = 0;
            totalElapsed = 0;
            paused = false;
        }

        protected void UpdateFrame(float elapsed)
        {
            if (paused)
                return;

            totalElapsed += elapsed;
            if (totalElapsed > timePerFrame)
            {
                frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                frame = frame % skins.Count;
                totalElapsed -= timePerFrame;
            }
        }

        // class AnimatedTexture
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, frame, screenPos);
        }

        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            //Rectangle sourcerect = new Rectangle(frameWidth * frame, 0, frameWidth, myTexture.Height);
            this.frame = frame;
            batch.Draw(skins[frame], screenPos, Color.White);
        }

        public bool IsPaused
        {
            get { return paused; }
        }
        public void Reset()
        {
            frame = 0;
            totalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            paused = false;
        }
        public void Pause()
        {
            paused = true;
        }

        public string Skin
        {
            get { return skins[frame].Name; }
        }

        public int Width
        {
            get { return skins[frame].Width; }
        }

        public int Height
        {
            get { return skins[frame].Height; }
        }
    }
}
