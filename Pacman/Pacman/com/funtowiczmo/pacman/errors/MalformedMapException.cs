using Pacman.com.funtowiczmo.pacman.map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.errors
{
    public class MalformedMapException : MapException
    {
        public MalformedMapException(Map map)
            : base(map, map.Name + " est malformée")
        {

        }
    }
}
