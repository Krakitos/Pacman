using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman.com.funtowiczmo.pacman.map;
using Pacman.com.funtowiczmo.pacman.map.signal;
using Pacman.com.funtowiczmo.pacman.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.view
{
    public class MapView : SpriteBatch, IObserver<MapSignal>
    {
        private Map map;
        private IDisposable mapListener;

        private bool needRefresh = true;

        public MapView(GraphicsDevice device):base(device){
               
        }

        public Map Map
        {
            get { return map; }
            set { 
                map = value;
                mapListener = map.Subscribe(this);
                needRefresh = true;
            }
        }

        public bool NeedRefresh
        {
            get { return needRefresh; }
            set { needRefresh = value; }
        }

        public void UpdateMap()
        {

            int[][] data = map.Grid;
            if (data.Length < 1) return;

            Rectangle rect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width/data[0].Length, GraphicsDevice.Viewport.Height/data.Length);
            

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    this.Draw(GetTextureFromID(data[i][j]), rect, Color.White);
                    if (data[i][j] == 2)
                    {
                        Rectangle beanRect = new Rectangle(rect.X, rect.Y, rect.Width*7/10, rect.Height*7/10);
                        beanRect.X += (rect.Width - beanRect.Width) >> 1;
                        beanRect.Y += (rect.Height - beanRect.Height) >> 1;
                        this.Draw(GetTextureFromID(-2), beanRect, Color.White);
                    }
                    
                    else if (data[i][j] == 1)
                    {
                        Rectangle beanRect = new Rectangle(rect.X, rect.Y, rect.Width >> 1, rect.Height >> 1);
                        beanRect.X += (rect.Width - beanRect.Width) >> 1;
                        beanRect.Y += (rect.Height - beanRect.Height) >> 1;
                        this.Draw(GetTextureFromID(-1), beanRect, Color.White);
                    }
                    rect.X += rect.Width;
                }

                rect.Y += rect.Height;
                rect.X = 0;
            }
        }

        private Texture2D GetTextureFromID(int id)
        {
            Texture2D t = null;

            switch (id)
            {
                case 0:
                    {

                        t = AssetsManager.GetInstance().GetTexture(entity.EntitySkinEnum.MUR);
                        break;
                    }
                case 1:
                case 2:
                case 3:
                    {
                        t = AssetsManager.GetInstance().GetTexture(entity.EntitySkinEnum.ROUTE);
                        break;
                    }

                case -1:
                    {
                        t = AssetsManager.GetInstance().GetTexture(entity.EntitySkinEnum.BEAN);
                        break;
                    }
                case -2:
                    {
                        t = AssetsManager.GetInstance().GetTexture(entity.EntitySkinEnum.GROS_BEAN);
                        break;
                    }

                    
            }

            return t;
        }


        public void OnCompleted()
        {
            mapListener.Dispose();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(MapSignal value)
        {
            if (value.GetType().Equals(typeof(MapBeanEatenSignal)))
            {

            }
        }
    }
}
