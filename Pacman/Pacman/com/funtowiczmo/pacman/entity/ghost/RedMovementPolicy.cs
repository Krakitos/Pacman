using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.entity.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.ghost
{
    public class RedMovementPolicy : IMovementPolicy
    {
        private IMovementPolicy runAway = new RunAwayMovementPolicy();

        public Vector2 GetDestination(MovableEntity entity, PacmanEntity pacman, map.Map map)
        {
            //Le fantome rouge cherche toujours à aller vers Pacman            
            if (pacman.IsGodMode)
            {
                return runAway.GetDestination(entity, pacman, map);
            }

            return pacman.Position;
        }
    }
}
