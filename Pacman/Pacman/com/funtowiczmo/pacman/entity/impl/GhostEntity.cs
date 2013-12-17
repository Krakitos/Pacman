using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.entity.ghost;
using Pacman.com.funtowiczmo.pacman.entity.signal;
using Pacman.com.funtowiczmo.pacman.map;
using System;
using System.Collections.Generic;
namespace Pacman.com.funtowiczmo.pacman.entity.impl
{
	public class GhostEntity : MovableEntity {

        private IMovementPolicy movementComputer;
        private bool pacmanWantsToKillHim;

        private Dictionary<bool, string[]> skins;
        

        public GhostEntity(string skin, IMovementPolicy movementPolicy) : base()
        {
            this.movementComputer = movementPolicy;
            this.skins = new Dictionary<bool, string[]>();
            skins.Add(false, new string[] { skin });
            skins.Add(true, new string[] { EntitySkinEnum.FANTOME_PEUR });
            pacmanWantsToKillHim = false;
        }

        public Vector2 ComputeNextMove(PacmanEntity pacman, Map map)
        {
            return movementComputer.GetDestination(this, pacman, map);
		}

        public override string[] GetDefaultSkins()
        {
            return skins[pacmanWantsToKillHim];
        }

        public bool AffraidMode
        {
            get{return pacmanWantsToKillHim;}
            set 
            { 
                pacmanWantsToKillHim = value;
                NotifyAll(new EntitySkinUpdated(this, skins[pacmanWantsToKillHim]));
            }
        }
    }

}
