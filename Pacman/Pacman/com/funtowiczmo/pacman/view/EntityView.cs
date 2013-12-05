using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman.com.funtowiczmo.pacman.entity;
using Pacman.com.funtowiczmo.pacman.entity.signal;
using Pacman.com.funtowiczmo.pacman.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.view
{
    public class EntityView : AnimatedTexture, IObserver<EntitySignal>
    {
        //Références vers l'entity et son écouteur d'évènement
        protected AbstractEntity entity;
        protected AbstractEntity.EntityListener listener;

        public EntityView(AbstractEntity entity)
            : base()
        {
            this.entity = entity;
            listener = (AbstractEntity.EntityListener)entity.Subscribe(this);
            Load(entity.GetDefaultSkins());
        }

        public void OnCompleted()
        {
            listener.Dispose();
            Stop();
        }

        public void OnError(Exception error)
        {
            //TODO : traiter une erreur si elle survient
            Console.Error.WriteLine(error);
        }

        public virtual void OnNext(EntitySignal value)
        {
            if (value.GetType() == typeof(EntitySkinUpdated))
            {
                EntitySkinUpdated esu = (EntitySkinUpdated)value;
                Load(esu.Skins, esu.Skins.Count());
            }
        }

        public void Update(float elapsed)
        {
            UpdateFrame(elapsed);
        }

        public AbstractEntity RelatedEntity
        {
            get { return entity; }
        }
    }
}
