using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.ghost
{
    public class RandomMovementPolicy : IMovementPolicy
    {
        private Random rand = new Random();

        public Vector2 GetNextMove(AbstractEntity target, Vector2 pacmanPos, map.Map map)
        {
            Vector2 next = new Vector2(target.Position.X, target.Position.Y);

            //do
            //{
            //    int val = rand.Next(-1, 1);

            //} while (map.IsNextMoveAuthorized(next));

            return next;
        }
    }
}
