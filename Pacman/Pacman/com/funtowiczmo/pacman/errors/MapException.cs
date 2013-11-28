using Pacman.com.funtowiczmo.pacman.map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.errors
{
    public abstract class MapException : SystemException
    {
        private Map map;

        public MapException(Map map, string message)
            : base(message)
        {
            this.map = map;
        }

        protected Map RelatedMap
        {
            get { return map; }
        }
    }
}
