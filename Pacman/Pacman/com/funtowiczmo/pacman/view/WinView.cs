using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman.com.funtowiczmo.pacman.entity;
using Pacman.com.funtowiczmo.pacman.utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.view
{
    public class WinView
    {
        private SpriteFont font;

        public WinView(SpriteFont font)
        {
            this.font = font;
        }

        public void Draw(SpriteBatch sprite)
        {
            const string text = "Vous avez gagné !";
            Viewport viewport = sprite.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            sprite.Draw(AssetsManager.GetInstance().GetTexture(EntitySkinEnum.ROUTE), fullscreen, Color.Black);

            Vector2 pos = new Vector2((fullscreen.Height - font.MeasureString(text).X) /2, (fullscreen.Height- font.MeasureString(text).Y) /2);
            sprite.DrawString(font, text, pos, Color.White);
        }
    }
}
