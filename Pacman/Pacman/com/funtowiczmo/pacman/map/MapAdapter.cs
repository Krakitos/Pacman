using Microsoft.Xna.Framework.Graphics;
using Pacman.com.funtowiczmo.pacman.map;
using Pacman.com.funtowiczmo.pacman.view;
using System;

namespace Pacman.com.funtowiczmo.pacman.entity
{
	public class MapAdapter {
		private Map map;

        public MapAdapter(Map map){
            this.map = map;
        }

        public void DrawMap(MapView view)
        {
            view.Map = map;
        }
	}

}
