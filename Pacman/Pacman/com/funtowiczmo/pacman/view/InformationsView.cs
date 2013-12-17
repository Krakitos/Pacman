using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman.com.funtowiczmo.pacman.entity;
using Pacman.com.funtowiczmo.pacman.entity.impl;
using Pacman.com.funtowiczmo.pacman.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.view
{
    public class InformationsView
    {
        private Texture2D background;
        private Texture2D heart;
        private SpriteFont font;

        public InformationsView(GraphicsDevice device, SpriteFont font)
        {
            this.font = font;
            InitBackground(device);
        }

        private void InitBackground(GraphicsDevice device)
        {
            background = new Texture2D(device, device.Viewport.Width, device.Viewport.Height);
            heart = AssetsManager.GetInstance().GetTexture(EntitySkinEnum.HEART);

            Color[] color = new Color[background.Width * background.Height];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.Black;
            }

            background.SetData<Color>(color);
        }

        public void Draw(SpriteBatch sprite, PacmanEntity pacman, Rectangle bounds)
        {

            sprite.Draw(background, bounds, Color.White);
            DrawTitle(sprite, bounds);

            DrawPoints(sprite, bounds, pacman.Points);
            DrawLife(sprite, pacman.RemainingLife, bounds);
        }

        private void DrawPoints(SpriteBatch sprite, Rectangle bounds, int points)
        {
            string points_desc = "Points : " + points;

            Vector2 pos = new Vector2(bounds.X + (bounds.Width - font.MeasureString(points_desc).X) / 2, 50);
            sprite.DrawString(font, points_desc, pos, Color.White);
        }

        private void DrawTitle(SpriteBatch sprite, Rectangle bounds)
        {
            Vector2 pos = new Vector2(bounds.X + (bounds.Width - font.MeasureString("Pacman").X) / 2, 20);
            sprite.DrawString(font, "Pacman", pos, Color.White);
        }

        private void DrawLife(SpriteBatch sprite, int remainingLife, Rectangle bounds)
        {
            string text = "Vie" + (remainingLife > 1 ? "s" : "");

            Vector2 tPos = new Vector2(bounds.X + (bounds.Width - font.MeasureString(text).X) / 2, 80);
            sprite.DrawString(font, text, tPos, Color.White);

            //La hauteur des coeurs est egale à la largeur, donc on a le même scale
            float scale = Math.Min(3f*heart.Width/(bounds.Width - 20), 1); 
            Vector2 hPos = new Vector2(bounds.X + ((bounds.Width -20) - 3*heart.Width) , 110);

            for (int i = 0; i < remainingLife; i++)
            {
                sprite.Draw(heart, hPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                hPos.X += heart.Width * scale;
            }
        }
    }
}
