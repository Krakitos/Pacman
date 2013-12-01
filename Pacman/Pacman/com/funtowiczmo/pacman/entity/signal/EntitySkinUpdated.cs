using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.signal
{
    public class EntitySkinUpdated : EntitySignal
    {
        private string[] skins;

        public EntitySkinUpdated(IEntity entity, string skin)
            : base(entity)
        {
            skins = new string[]{skin};
        }

        public EntitySkinUpdated(IEntity entity, string[] skins):base(entity)
        {
            this.skins = skins;
        }

        public string[] Skins
        {
            get { return skins; }
        }
    }
}
