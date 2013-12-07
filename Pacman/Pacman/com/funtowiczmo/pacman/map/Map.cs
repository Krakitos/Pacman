using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.errors;
using Pacman.com.funtowiczmo.pacman.map.signal;
using System;
using System.Collections.Generic;
using System.IO;
namespace Pacman.com.funtowiczmo.pacman.map
{
    /// <summary>
    /// Classe répresentant la structure interne d'une map de jeu
    /// </summary>
	public class Map : IObservable<MapSignal>{
        private List<IObserver<MapSignal>> observers;

        private int[][] grid;
        private string name;

        private int beanItemsCount = 0;
        private int bigBeanItemsCount = 0;

		public Map(string name) {
            this.name = name;
            observers = new List<IObserver<MapSignal>>();
		}

        public Map(string name, int[][] grid)
        {
            this.name = name;
            this.grid = grid;
            observers = new List<IObserver<MapSignal>>();
		}

        /// <summary>
        /// Renvoie la structure interne de la grille
        /// </summary>
        public int[][] Grid
        {
            get { return grid; }
        }

        /// <summary>
        /// Renvoie le nom de la map
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Renvoie le nombre de haricots encore disponible sur la map actuelle
        /// </summary>
        public int BeanCount
        {
            get { return beanItemsCount; }
        }

        /// <summary>
        /// Renvoie le nombre de haricots spéciaux encore disponible sur la map actuelle
        /// </summary>
        public int BigBeanCount
        {
            get { return bigBeanItemsCount; }
        }

        /// <summary>
        /// Charge la map en mémoire et la lit
        /// </summary>
        public void Load()
        {
            string[] data;

            try
            {
                data = File.ReadAllLines("Content/maps/" + name);
            }catch(SystemException){
                throw new MapNotFoundException(this, "Content/maps/"+name);
            }

            if (data == null)
            {
                throw new System.Exception("Unable to read map : maps/" + name);
            }

            grid = new int[data.Length][];

            for (int i = 0; i < data.Length; i++)
            {
                string[] cellid = data[i].Split(',');
                grid[i] = new int[cellid.Length];

                for (int j = 0; j < cellid.Length; j++)
                {
                    grid[i][j] = Int32.Parse(cellid[j]);

                    switch (grid[i][j])
                    {
                        case 1 :
                            ++beanItemsCount;
                            break;

                        case 2 :
                            ++bigBeanItemsCount;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Regarde la value contenu à la position [x][y] dans la grille, et informe la vue de l'entité présente à cette position
        /// </summary>
        /// <param name="pos"></param>
        public void CheckPosition(Vector2 pos)
        {
            int val = grid[(int)pos.Y][(int)pos.X];

            switch (val)
            {
                case 1:
                case 2:
                    {
                        bool bigBean = false;

                        if (grid[(int)pos.Y][(int)pos.X] == 1)
                        {
                            --beanItemsCount;
                        }
                        else
                        {
                            bigBean = true;
                            --bigBeanItemsCount;
                        }

                        grid[(int)pos.Y][(int)pos.X] *= -1;
                        
                        DispatchSignal(new MapBeanEatenSignal(this, pos,  bigBean));

                        if (bigBeanItemsCount == 0 && beanItemsCount == 0)
                        {
                            DispatchSignal(new MapAllBeansEatenSignal(this));
                        }

                        break;
                    }
            }
        }

        protected void DispatchSignal(MapSignal signal)
        {
            foreach(IObserver<MapSignal> o in observers){
                o.OnNext(signal);
            }
        }

        public IDisposable Subscribe(IObserver<MapSignal> observer)
        {
            observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<MapSignal>> _observers;
            private IObserver<MapSignal> _observer;

            public Unsubscriber(List<IObserver<MapSignal>> observers, IObserver<MapSignal> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        // <summary>
        /// Indique si un mouvement vers le point p est autorisé
        /// </summary>
        /// <param name="x">Position en x du prochain mouvement</param>
        /// <param name="y">Position en y du prochain mouvement</param>
        /// <returns>True si le mouvement est possible, false sinon</returns>
        public bool IsNextMoveAuthorized(int x, int y)
        {
            return grid[y][x] != 0;
        }

        /// <summary>
        /// Indique si un mouvement vers le point p est autorisé
        /// </summary>
        /// <param name="p">Point 2D vers lequel on souhaite aller</param>
        /// <returns>True si le mouvement est possible, false sinon</returns>
        public bool IsNextMoveAuthorized(Vector2 p)
        {
            return grid[(int)p.Y][(int)p.X] != 0;
        }

        /// <summary>
        /// Renvoie une position aléatoire sur la map pour le démarrage du Pacman
        /// </summary>
        /// <returns>Un vecteur(x,y) indiquant la position en (x,y)</returns>
        public Vector2 GetRandomInitialPacmanPosition()
        {
            int x, y;
            Random rand = new Random(System.DateTime.Now.Millisecond);

            do
            {
                y = rand.Next(1, grid.Length - 1);
                x = rand.Next(1, grid[y].Length - 1);

            } while (grid[y][x] != 1);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Renvoie une position aléatoire dans la zone de démarrage pour les fantomes
        /// </summary>
        /// <returns>Un vecteur(x,y) indiquant la position en (x,y)</returns>
        public Vector2 GetRandomInitialGhostPosition()
        {
            int x, y;
            Random rand = new Random(System.DateTime.Now.Millisecond);
            do
            {
                y = rand.Next(1, grid.Length - 1);
                x = rand.Next(1, grid[y].Length - 1);

            } while (grid[y][x] != 3);

            grid[y][x] = -3;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Permet de déterminer si le point (x,y) est à une intersection sur la map
        /// </summary>
        /// <param name="pos">Contient un vecteur représentant le point dans l'espace 2D</param>
        /// <returns>Vrai si le point correspond à une intersection, false sinon</returns>
        public bool IsIntersection(Vector2 pos)
        {
            return IsIntersection((int)pos.X, (int)pos.Y);
        }

        /// <summary>
        /// Permet de déterminer si le point (x,y) est à une intersection sur la map
        /// </summary>
        /// <param name="x">Composante x</param>
        /// <param name="y">Composante y</param>
        /// <returns>Vrai si le point correspond à une intersection, false sinon</returns>
        public bool IsIntersection(int x, int y)
        {
            int availableDirectionCount = 0;

            if (x + 1 != 0 && x + 1 != 3 && x + 1 != -3)
            {
                availableDirectionCount++;
            }

            if (x - 1 != 0 && x - 1 != 3 && x - 1 != -3)
            {
                availableDirectionCount++;
            }

            if (y + 1 != 0 && y + 1 != 3 && y + 1 != -3)
            {
                availableDirectionCount++;
            }

            if (y - 1 != 0 && y - 1 != 3 && y - 1 != -3)
            {
                availableDirectionCount++;
            }

            return availableDirectionCount >= 2;
        }

        /// <summary>
        /// Permet de déterminer si deux entitées peuvent se voir (pas de mur ou de zone de respawn entre elles)
        /// </summary>
        /// <param name="first">Position de la première entité</param>
        /// <param name="second">Position de la seconde entité</param>
        /// <returns>Renvoie true si les deux entitées peuvent se voir, false sinon</returns>
        public bool CanSee(Vector2 first, Vector2 second)
        {
            bool res = true;

            int y = (int)first.Y;
            int y_inc = second.Y - first.Y < 0 ? -1 : 1;

            int x = (int)first.X;
            int x_inc = second.X - first.X < 0 ? -1 : 1;

            while (y < second.Y && res)
            {
                if (grid[y][x] == 0 || grid[y][x] == 3 ||grid[y][x] == -3)
                {
                    res = false;
                }
                else
                {
                    y += y_inc;
                }
            }

            y = (int)first.Y;

            while (x < second.X && res)
            {
                if (grid[y][x] == 0 || grid[y][x] == 3 || grid[y][x] == -3)
                {
                    res = false;
                }else{
                    x += x_inc;
                }
            }

            return res;
        }
    }

}
