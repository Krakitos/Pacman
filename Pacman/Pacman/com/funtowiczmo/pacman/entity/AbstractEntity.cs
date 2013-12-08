using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.entity.signal;
using Pacman.com.funtowiczmo.pacman.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity
{
    public abstract class AbstractEntity : IEntity
    {
        private static int ID_GENERATOR = 0;
        private int id;
        private List<IObserver<EntitySignal>> observers; 

        //Position sur la map
        protected Vector2 position;

        public AbstractEntity()
        {
            id = ++ID_GENERATOR;
            observers = new List<IObserver<EntitySignal>>();
        }

        public int GetID()
        {
            return id;
        }

        public abstract string[] GetDefaultSkins();

        public Vector2 SetInitialPosition(float x, float y)
        {
            position = new Vector2(x, y);
            return position;
        }

        public Vector2 SetInitialPosition(Vector2 pos)
        {
            position = pos;
            return position;
        }

        /// <summary>
        /// Représente la position de Pacman dans le référentiel de la map. Ces points ne correspondent pas à des coordonées d'affichage à l'écran.
        /// Pour cela utiliser la fonction MapView.ConvertPointToScreenPoint(int x, int y).
        /// </summary>
        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public IDisposable Subscribe(IObserver<EntitySignal> observer)
        {
            observers.Add(observer);
            EntityListener listener = new EntityListener(observers, observer);

            return listener;
        }

        /// <summary>
        /// Envoie un message à tous les observateurs, les informants d'un nouvel évèmenet
        /// </summary>
        /// <param name="signal">L'évènement à envoyer</param>
        public void NotifyAll(EntitySignal signal)
        {
            foreach (IObserver<EntitySignal> o in observers)
            {
                o.OnNext(signal);
            }
        }

        /// <summary>
        /// Envoie un message à tous les observateurs les informants d'une erreur
        /// </summary>
        /// <param name="e">L'erreur à transmettre</param>
        public void NotifyAll(Exception e)
        {
            foreach (IObserver<EntitySignal> o in observers)
            {
                o.OnError(e);
            }
        }

        /// <summary>
        /// Envoie un message à tous les observateurs les informants qu'ils peuvent se désabonner de cette objet
        /// </summary>
        public void NotifyAll()
        {
            foreach (IObserver<EntitySignal> o in observers)
            {
                o.OnCompleted();
            }
        }

        //Classe gérant ke désabonnement aux évènements
        public class EntityListener : IDisposable
        {
            private List<IObserver<EntitySignal>> observers;
            private IObserver<EntitySignal> observer;

            public EntityListener(List<IObserver<EntitySignal>> observers, IObserver<EntitySignal> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (observer != null && observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
        }
    }
}
