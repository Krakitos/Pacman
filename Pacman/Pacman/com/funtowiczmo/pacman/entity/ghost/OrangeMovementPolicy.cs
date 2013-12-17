using Pacman.com.funtowiczmo.pacman.entity.impl;
using Pacman.com.funtowiczmo.pacman.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.ghost
{
    public class OrangeMovementPolicy : IMovementPolicy
    {
        private IMovementPolicy runAway = new RunAwayMovementPolicy();

        public Microsoft.Xna.Framework.Vector2 GetDestination(MovableEntity entity, PacmanEntity pacman, map.Map map)
        {
            //Si pacman est en mode invulnérable on s'en va !
            if (pacman.IsGodMode)
            {
                runAway.GetDestination(entity, pacman, map);
            }

            //Si on est dans le berceau on s'en sort
            if (map.IsInTheCradle(entity.Position))
            {
                return pacman.Position;
            }

            //Si on voit Pacman, on le rush !
            if (map.CanSee(entity.Position, pacman.Position))
            {
                return pacman.Position;
            }

            //On doit calculer le prochain mouvement en fonction des directions disponibles
            EntityDirectionEnum nextDir;

            List<EntityDirectionEnum> directions = map.GetAvailableDirections(entity.Position);
            if (directions.Contains(entity.Direction))
            {
                nextDir = entity.Direction;
            }
            else
            {
                //On retire le retour en arrière pour eviter les effet de va et viens, si une autre direction est possible (directions.Count > 1)
                if (directions.Count > 1)
                {
                    directions.Remove(MapUtils.getOppositeDirection(entity.Direction));
                }
                nextDir = directions.ElementAt(MathUtils.Random(0, directions.Count - 1));
            }

            return MapUtils.GetNextPointWithDirection(entity.Position, nextDir);
        }
    }
}
