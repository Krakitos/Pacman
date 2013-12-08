using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.ghost
{
    public class ShortestPathPolicy : IMovementPolicy
    {
        public Vector2 GetDestination(MovableEntity entity, Vector2 pacmanPos, map.Map map)
        {
            //Le fantome rouge cherche toujours à aller vers Pacman
            return pacmanPos;
        }
    }
}
