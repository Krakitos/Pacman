using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.ghost
{
    interface IMovementPolicy
    {
        /// <summary>
        /// Renvoie un entier identifiant dans quel direction doit aller le fantome dans son prochain mouvement
        /// </summary>
        /// <param name="pacmanPos"></param>
        /// <returns>0 = droite, 1 = gauche, 2 = haut, 3 = bas</returns>
        int GetNextMove(Point pacmanPos, Map map);
    }
}
