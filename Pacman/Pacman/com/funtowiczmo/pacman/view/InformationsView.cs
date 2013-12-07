using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman.com.funtowiczmo.pacman.entity.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.view
{
    public class InformationsView
    {
        private Texture2D background;
        private SpriteFont font;

        public InformationsView(GraphicsDevice device, SpriteFont font)
        {
            this.font = font;
            InitBackground(device);
        }

        private void InitBackground(GraphicsDevice device)
        {
            background = new Texture2D(device, device.Viewport.Width, device.Viewport.Height);

            Color[] color = new Color[background.Width * background.Height];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.Black;
            }
            
            background.SetData<Color>(color);
        }

        public void UpdateInformation(SpriteBatch sprite, PacmanEntity pacman, Rectangle bounds)
        {

            sprite.Draw(background, bounds, Color.White);
            DrawTitle(sprite, bounds);

            DrawPoints(sprite, bounds, pacman.Points);
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
    }
}
