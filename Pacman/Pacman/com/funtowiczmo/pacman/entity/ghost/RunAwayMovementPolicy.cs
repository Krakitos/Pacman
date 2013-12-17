using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.ghost
{
    public class RunAwayMovementPolicy : IMovementPolicy
    {
        public Microsoft.Xna.Framework.Vector2 GetDestination(MovableEntity entity, impl.PacmanEntity pacman, map.Map map)
        {
            //On récupère les directions disponibles depuis notre position
            List<EntityDirectionEnum> availableDirections = map.GetAvailableDirections(entity.Position);

            Vector2 nextPos;
            EntityDirectionEnum nextDir;

            if (!availableDirections.Contains(entity.Direction))
            {
                //On récupère une direction au hasard dans celle dispo en évitant celle de Pacman si possible
                if (availableDirections.Count > 1)
                {
                    availableDirections.Remove(pacman.Direction);
                }

                nextDir = availableDirections.ElementAt(MathUtils.Random(0, availableDirections.Count)); 
            }
            else
            {
                nextDir = entity.Direction;
            }
           

            //On calcule le prochain point à partir de la position actuelle et de la direction
            nextPos = MapUtils.GetNextPointWithDirection(entity.Position, nextDir);

            //On renvoie la position
            return nextPos;
        }
    }
}
