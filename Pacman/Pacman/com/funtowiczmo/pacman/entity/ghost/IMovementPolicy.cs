using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.ghost
{
    public interface IMovementPolicy
    {
        /// <summary>
        /// Renvoie un entier identifiant dans quel direction doit aller le fantome dans son prochain mouvement
        /// </summary>
        /// <param name="entity">Réference vers l'entité qui effectue le déplacement</param>
        /// <param name="pacmanPos">Position de Pacman</param>
        /// <param name="map">Référence vers la map</param>
        /// <returns>0 = droite, 1 = gauche, 2 = haut, 3 = bas</returns>
        Vector2 GetNextMove(AbstractEntity entity, Vector2 pacmanPos, Map map);
    }
}
