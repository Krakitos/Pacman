using Pacman.com.funtowiczmo.pacman.entity.impl;
using Pacman.com.funtowiczmo.pacman.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.ghost
{
    public class PinkMovementPolicy : IMovementPolicy
    {
        private IMovementPolicy runAway = new RunAwayMovementPolicy();

        public Microsoft.Xna.Framework.Vector2 GetDestination(MovableEntity entity, PacmanEntity pacman, map.Map map)
        {
            //Si l'on est dans le berceau on en sort en se dirigeant vers Pacman (car c'est le seul à ne pas être dedans normalement)
            if (map.IsInTheCradle(entity.Position))
            {
                return pacman.Position;
            }

            //Si Pacman est en mode invulnérable, on le fuit
            if (pacman.IsGodMode)
            {
                return runAway.GetDestination(entity, pacman, map);
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
                nextDir = directions.ElementAt(MathUtils.Random(0, directions.Count));
            }

            return MapUtils.GetNextPointWithDirection(entity.Position, nextDir);
        }
    }
}
