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
            //Pour sortir du berceau on utilise la position de pacman
            if (map.IsInTheCradle(entity.Position))
            {
                return pacman.Position;
            }

            //Si on est proche d'un gros haricot, machine arrière !!!
            if (map.isNextToBigBean(pacman.Position))
            {
                return runAway.GetDestination(entity, pacman, map);
            }

            //Sinon on récupère les directions disponibles
            List<EntityDirectionEnum> directions = map.GetAvailableDirections(entity.Position);
            EntityDirectionEnum nextDir;

            //Si notre direction actuelle est dispo, on continue notre chemin
            if (directions.Contains(entity.Direction))
            {
                nextDir = entity.Direction;
            }
            else
            {
                //S'il ne reste qu'une seule possibilité, on ne va pas la retirer sous peine de bloquer l'entité
                if (directions.Count > 1)
                {
                    directions.Remove(MapUtils.getOppositeDirection(entity.Direction));
                }
                
                //Aléatoire sur la prochaine direction
                nextDir = directions.ElementAt(MathUtils.Random(0, directions.Count));
            }

            //Sinon on vie sa vie
            return MapUtils.GetNextPointWithDirection(entity.Position, nextDir);
        }
    }
}
