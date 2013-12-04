using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.signal
{
    public class PacmanGodModeSignal : EntitySignal
    {
        private bool godMode;

        public PacmanGodModeSignal(IEntity entity, bool godMode) : base(entity)
        {
            this.godMode = godMode;
        }

        public bool IsGod
        {
            get { return godMode; }
        }
    }
}
