using Pacman.com.funtowiczmo.pacman.utils.signal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.signal
{
    public abstract class EntitySignal : Signal
    {
        private IEntity target;

        public EntitySignal(IEntity target)
        {
            this.target = target;
        }

        public IEntity Target
        {
            get { return target; }
        }
    }
}
