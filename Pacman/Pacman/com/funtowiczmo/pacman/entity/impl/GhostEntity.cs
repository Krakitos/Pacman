using System;
namespace Pacman.com.funtowiczmo.pacman.entity.impl
{
	public class GhostEntity : AbstractEntity {

        public static int GHOST_VELOCITY = 1;

        public GhostEntity(string skin) : base()
        {
            
        }

		public void ComputeNextMove() {
			throw new System.Exception("Not implemented");
		}
    }

}
