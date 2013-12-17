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
                //On retire le retour en arrière pour eviter les effet de va et viens, si une autre direction est possible (directions.Count > 1)
                if (directions.Count > 1)
                {
                    switch (entity.Direction)
                    {
                        case EntityDirectionEnum.BOTTOM:
                            directions.Remove(EntityDirectionEnum.TOP);
                            break;
                        case EntityDirectionEnum.TOP:
                            directions.Remove(EntityDirectionEnum.BOTTOM);
                            break;
                        case EntityDirectionEnum.LEFT:
                            directions.Remove(EntityDirectionEnum.RIGHT);
                            break;
                        case EntityDirectionEnum.RIGHT:
                            directions.Remove(EntityDirectionEnum.LEFT);
                            break;
                    }
                }

                nextDir = directions.ElementAt(MathUtils.Random(0, directions.Count));
            }

            return MapUtils.GetNextPointWithDirection(entity.Position, nextDir);
        }
    }
}
