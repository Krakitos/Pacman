using Pacman.com.funtowiczmo.pacman.entity;
using Pacman.com.funtowiczmo.pacman.entity.signal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.view.impl
{
    public class PacmanView : EntityView
    {
        public PacmanView(IEntity entity)
            : base(entity)
        {

        }

        public override void OnNext(EntitySignal value)
        {
            if (value.GetType() == typeof(PacmanGodModeSignal))
            {
                //Dispatcher l'event vers le controler global pour mettre à jour les fantomes
            }
            else
            {
                base.OnNext(value);
            }
        }
    }
}
