using System;
namespace Pacman.com.funtowiczmo.pacman.sound
{
	public class SoundManager {

        private static SoundManager instance = new SoundManager();

        private bool available;

        private SoundManager()
        {
            available = true;
        }

        public bool Available
        {
            get { return available; }
            set { available = value; }
        }

		public bool Play(int soundID) {
			throw new System.Exception("Not implemented");
		}

        public static SoundManager GetInstance()
        {
            return instance;
        }

		private SoundEnum soundEnum;
	}

}
