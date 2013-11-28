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

        public MapBeanEatenSignal(Map map, Vector2 pos)
            : base(map)
        {
            this.pos = pos;
        }

        public Vector2 Position
        {
            get { return pos; }
        }
    }
}
