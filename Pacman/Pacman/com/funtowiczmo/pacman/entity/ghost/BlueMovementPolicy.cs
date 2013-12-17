using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.entity.impl;
using Pacman.com.funtowiczmo.pacman.map;
using Pacman.com.funtowiczmo.pacman.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.ghost
{
    public class BlueMovementPolicy : IMovementPolicy
    {

        private IMovementPolicy runAway = new RunAwayMovementPolicy();

        public Vector2 GetDestination(MovableEntity entity, PacmanEntity pacman, Map map)
        {
            //Si on est proche d'un gros haricot, machine arrière !!!
            if (map.isNextToBigBean(pacman.Position))
            {
                return runAway.GetDestination(entity, pacman, map);
            }

            //Sinon on vie sa vie
            return MapUtils.GetNextPointWithDirection(entity.Position, entity.Direction);
        }
    }
}
