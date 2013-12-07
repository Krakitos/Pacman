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
    public class MapView : IObserver<MapSignal>
    {
        private Map map;
        private IDisposable mapListener;

        //Reference pour une unité de mesure en X et en Y
        private Rectangle unit;

        private bool needRefresh = true;

        public MapView():base(){
               
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

        public void UpdateMap(SpriteBatch sprite, float width, float height)
        {

            int[][] data = map.Grid;
            if (data.Length < 1) return;

            //unit = new Rectangle(0, 0, sprite.GraphicsDevice.Viewport.Width / data[0].Length, sprite.GraphicsDevice.Viewport.Height / data.Length);
            unit = new Rectangle(0, 0, (int)Math.Floor(width / data[0].Length), (int)Math.Floor(height / data.Length));
            Rectangle rect = new Rectangle(0, 0, unit.Width, unit.Height);
            

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    sprite.Draw(GetTextureFromID(data[i][j]), rect, Color.White);
                    if (data[i][j] == 2)
                    {
                        Rectangle beanRect = new Rectangle(rect.X, rect.Y, rect.Width*6/10, rect.Height*6/10);
                        beanRect.X += (rect.Width - beanRect.Width) >> 1;
                        beanRect.Y += (rect.Height - beanRect.Height) >> 1;
                        sprite.Draw(AssetsManager.GetInstance().GetTexture(entity.EntitySkinEnum.GROS_BEAN), beanRect, Color.White);
                    }
                    
                    else if (data[i][j] == 1)
                    {
                        Rectangle beanRect = new Rectangle(rect.X, rect.Y, rect.Width >> 1, rect.Height >> 1);
                        beanRect.X += (rect.Width - beanRect.Width) >> 1;
                        beanRect.Y += (rect.Height - beanRect.Height) >> 1;
                        sprite.Draw(AssetsManager.GetInstance().GetTexture(entity.EntitySkinEnum.BEAN), beanRect, Color.White);
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
                case -1:
                case -2:
                case -3:
                    {
                        t = AssetsManager.GetInstance().GetTexture(entity.EntitySkinEnum.ROUTE);
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
            
        }

        public bool IsNextMoveAuthorized(int x, int y){
            return map.IsNextMoveAuthorized(x, y);
        }

        /// <summary>
        /// Converti un point de la map (x,y) en un point de coordonnée dans le référéntiel de l'écran.
        /// </summary>
        /// <param name="x">Positon en x sur la map</param>
        /// <param name="y">Position en y sur la map</param>
        /// <returns>{x,y}</returns>
        public Vector2 ConvertPointToScreenPoint(float x, float y)
        {
            return new Vector2(x * unit.Width, y * unit.Height);
        }

        /// <summary>
        /// Converti un point de la map (x,y) en un point de coordonnée dans le référéntiel de l'écran.
        /// </summary>
        /// <param name="p">Vector contenant la position 2D</param>
        /// <returns>{x,y}</returns>
        public Vector2 ConvertPointToScreenPoint(Vector2 p)
        {
            return ConvertPointToScreenPoint(p.X, p.Y);
        }

        public int Width
        {
            get { return unit.Width * map.Grid[0].Length; }
        }

        public int Height
        {
            get { return unit.Height * map.Grid.Length; }
        }
    }
}
