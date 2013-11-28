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
        //private Texture2D myTexture;
        private float TimePerFrame;
        private int Frame;
        private float TotalElapsed;
        private bool Paused;
        private List<Texture2D> skins;

        public float Scale;

        public AnimatedTexture()
        {
            this.Scale = 1;
        }

        public AnimatedTexture(float scale)
        {
            this.Scale = scale;
        }

        public void Load(string asset, int framesPerSec)
        {
            skins = new List<Texture2D>();

            skins.Add(AssetsManager.GetInstance().GetTexture(asset));
            TimePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        public void Load(string[] assets, int framesPerSec)
        {
            skins = new List<Texture2D>(assets.Length);

            for (int i = 0; i < assets.Length; i++) skins.Add(AssetsManager.GetInstance().GetTexture(assets[i]));

            TimePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        public void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % skins.Count;
                TotalElapsed -= TimePerFrame;
            }
        }

        // class AnimatedTexture
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, Frame, screenPos);
        }

        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            //Rectangle sourcerect = new Rectangle(frameWidth * frame, 0, frameWidth, myTexture.Height);
            Frame = frame;
            batch.Draw(skins[Frame], screenPos, Color.White);
        }

        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }

        public string Skin
        {
            get { return skins[Frame].Name; }
        }

    }
}
