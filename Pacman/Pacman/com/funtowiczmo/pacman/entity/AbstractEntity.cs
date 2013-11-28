using Pacman.com.funtowiczmo.pacman.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity
{
    public abstract class AbstractEntity : IEntity
    {
        private int ID_GENERATOR = 0;
        private int id;

        private AnimatedTexture view;

        public AbstractEntity()
        {
            id = ++ID_GENERATOR;
            view = new AnimatedTexture();
        }

        public AnimatedTexture View
        {
            get { return view; }
            set { view = value; }
        }

        public int GetID()
        {
            return id;
        }
    }
}
