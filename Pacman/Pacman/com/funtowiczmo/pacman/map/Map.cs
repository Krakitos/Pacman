using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.entity;
using Pacman.com.funtowiczmo.pacman.errors;
using Pacman.com.funtowiczmo.pacman.map.signal;
using Pacman.com.funtowiczmo.pacman.utils;
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
            int val = GetValueAt(pos.X, pos.Y);

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

        /// <summary>
        /// Renvoie la valeur à la position (x,y)
        /// </summary>
        /// <param name="x">Position en x</param>
        /// <param name="y">Position en y</param>
        /// <returns></returns>
        public int GetValueAt(int x, int y)
        {
            return grid[y][x];
        }

        /// <summary>
        /// Renvoie la valeur à la position (x,y)
        /// </summary>
        /// <param name="x">Position en x</param>
        /// <param name="y">Position en y</param>
        /// <returns></returns>
        public int GetValueAt(float x, float y)
        {
            return GetValueAt((int)x, (int)y);
        }

        /// <summary>
        /// Renvoie les directions possibles depuis un point pos
        /// </summary>
        /// <param name="pos">La position à partir de laquelle on souhaite déterminer les chemins possibles</param>
        /// <returns>Une liste d'EntityDirectionEnum</returns>
        public List<EntityDirectionEnum> GetAvailableDirections(Vector2 pos)
        {
            return GetAvailableDirections((int)pos.X, (int)pos.Y);
        }

        /// <summary>
        /// Renvoie les directions possibles depuis un point (x,y)
        /// </summary>
        /// <param name="x">Coordonées en x du point à partir duquel on souhaite déterminer les chemins</param>
        /// <param name="y">Coordonnées en y du point à partir duquel on souhaite déterminer les chemins</param>
        /// <returns>Une liste d'EntityDirectionEnum</returns>
        public List<EntityDirectionEnum> GetAvailableDirections(int x, int y)
        {
            List<EntityDirectionEnum> directionsAvailable = new List<EntityDirectionEnum>();

            if (IsNextMoveAuthorized(x + 1, y))
            {
                directionsAvailable.Add(EntityDirectionEnum.RIGHT);
            }

            if (IsNextMoveAuthorized(x - 1, y))
            {
                directionsAvailable.Add(EntityDirectionEnum.LEFT);
            }

            if (IsNextMoveAuthorized(x, y + 1))
            {
                directionsAvailable.Add(EntityDirectionEnum.BOTTOM);
            }

            if (IsNextMoveAuthorized(x, y - 1))
            {
                directionsAvailable.Add(EntityDirectionEnum.TOP);
            }

            return directionsAvailable;
        }

        /// <summary>
        /// Indique si un mouvement vers les coordonnées (x,y) est autorisé
        /// </summary>
        /// <param name="x">Composante x du vecteur position</param>
        /// <param name="y">Composante y du vecteur position</param>
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

            do
            {
                y = MathUtils.Random(1, grid.Length - 1);
                x = MathUtils.Random(1, grid[y].Length - 1);

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
            do
            {
                y = MathUtils.Random(1, grid.Length - 1);
                x = MathUtils.Random(1, grid[y].Length - 1);

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

            //On cherche si sur la même ligne / colonne un obstacle est préset. Si c'est le cas, les deux positions sont cachées l'une de l'autre. Sinon elles sont visibles
            bool res = true;

            int y = (int)first.Y;
            int y_inc = second.Y - first.Y < 0 ? -1 : 1;

            int x = (int)first.X;
            int x_inc = second.X - first.X < 0 ? -1 : 1;

            //On parcourt toute la ligne
            while (y < second.Y && res)
            {
                // 0 = mur, 3 | -3 = zone de respawn des fantomes 
                if (grid[y][x] == 0 || grid[y][x] == 3 ||grid[y][x] == -3)
                {
                    res = false;
                }
                else
                {
                    y += y_inc;
                }
            }

            //On reset le Y pour rester sur la même ligne
            y = (int)first.Y;

            //On parcourt toute la colonne;
            while (x < second.X && res)
            {
                // 0 = mur, 3 | -3 = zone de respawn des fantomes 
                if (grid[y][x] == 0 || grid[y][x] == 3 || grid[y][x] == -3)
                {
                    res = false;
                }else{
                    x += x_inc;
                }
            }

            return res;
        }

        /// <summary>
        /// Indique si la position est dans la zone de respawn des fantômes
        /// </summary>
        /// <param name="pos">Position de l'entité</param>
        /// <returns>True si cette position est dans la zone de respawn des fantômes, false sinon</returns>
        public bool IsInTheCradle(Vector2 pos)
        {
            return IsInTheCradle((int)pos.X, (int)pos.Y);
        }

        /// <summary>
        /// Indique si la position est dans la zone de respawn des fantômes
        /// </summary>
        /// <param name="x">Composante x de la position</param>
        /// <param name="y">Composante y de la position</param>
        /// <returns>True si cette position est dans la zone de respawn des fantômes, false sinon</returns>
        public bool IsInTheCradle(int x, int y)
        {
            return grid[y][x] == 3 || grid[y][x] == -3;
        }

        public bool isNextToBigBean(Vector2 pos)
        {
            int x = (int)Math.Max(0, pos.X - 1);
            int y = (int)Math.Max(0, pos.Y - 1);
            int xMax = (int)Math.Min(x + 3, grid[y].Length - 1);
            int yMax = (int)Math.Min(y + 3, grid.Length - 1);

            for (; x < xMax; x++)
            {
                for (; y < yMax; y++)
                {
                    if (grid[y][x] == 3) return true;
                }
            }

            return false;
        }
    }

}
