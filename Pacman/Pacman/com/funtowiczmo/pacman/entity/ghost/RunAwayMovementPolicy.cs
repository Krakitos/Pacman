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

            //On retire la direction de Pacman pour ne pas aller vers lui
            availableDirections.Remove(pacman.Direction);

            //On récupère une direction au hasard
            EntityDirectionEnum nextDir = availableDirections.ElementAt(MathUtils.Random(0, availableDirections.Count));

            //On calcule le prochain point à partir de la position actuelle et de la direction
            Vector2 nextPos = MapUtils.GetNextPointWithDirection(entity.Position, nextDir);

            //On renvoie la position
            return nextPos;
        }
    }
}
