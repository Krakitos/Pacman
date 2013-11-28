using Pacman.com.funtowiczmo.pacman.utils.signal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.map.signal
{
    public class MapSignal : Signal
    {
        protected Map map;

        public MapSignal(Map map)
        {
            this.map = map;
        }

        public Map Map
        {
            get { return map; }
        }
    }
}
