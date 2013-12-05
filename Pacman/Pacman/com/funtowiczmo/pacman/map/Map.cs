using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.errors;
using Pacman.com.funtowiczmo.pacman.map.signal;
using System;
using System.Collections.Generic;
using System.IO;
namespace Pacman.com.funtowiczmo.pacman.map
{
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

        public int[][] Grid
        {
            get { return grid; }
        }

        public string Name
        {
            get { return name; }
        }

        public int BeanCount
        {
            get { return beanItemsCount; }
        }

        public int BigBeanCount
        {
            get { return bigBeanItemsCount; }
        }

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

        public bool IsNextMoveAuthorized(int x, int y)
        {
            return grid[y][x] != 0;
        }

        public bool IsNextMoveAuthorized(Vector2 p)
        {
            return grid[(int)p.Y][(int)p.X] != 0;
        }

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
    }

}
