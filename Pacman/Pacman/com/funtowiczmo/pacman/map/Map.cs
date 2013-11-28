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
                }
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
    }

}
