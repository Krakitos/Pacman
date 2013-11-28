using Pacman.com.funtowiczmo.pacman.map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.errors
{
    class MapNotFoundException : MapException
    {
        private Map map; 
        private string path;

        public MapNotFoundException(Map map, string path)
            : base(map, "Impossible de charger la map " + map.Name)
        {
            this.path = path;
        }

        public string GetPath
        {
            get { return path; }
        }

    }
}
