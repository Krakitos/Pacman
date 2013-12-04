using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.map.signal
{
    public class MapBeanEatenSignal : MapSignal
    {
        private Vector2 pos;
        private bool bigBean;

        public MapBeanEatenSignal(Map map, Vector2 pos, bool bigBean)
            : base(map)
        {
            this.pos = pos;
            this.bigBean = bigBean;
        }

        public bool IsBigBean
        {
            get { return bigBean; }
        }

        public Vector2 Position
        {
            get { return pos; }
        }
    }
}
