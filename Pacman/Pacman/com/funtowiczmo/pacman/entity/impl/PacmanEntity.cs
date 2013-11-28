using Pacman.com.funtowiczmo.pacman.utils;
using System;
namespace Pacman.com.funtowiczmo.pacman.entity.impl
{
	public class PacmanEntity : AbstractEntity {

        private int points;
        private bool isGodMode;
        private EntityDirectionEnum direction;

        public PacmanEntity():base()
        {
            isGodMode = false;
            points = 0;
        }

        public void AddPoints(int p)
        {
            points += p;
        }

        public int Points
        {
            get { return points; }
        }

        public bool IsGodMode
        {
            get { return isGodMode; }
            set { isGodMode = value; }
        }

        public EntityDirectionEnum Direction
        {
            get { return direction; }
            set { direction = value; }
        }
    }

}
