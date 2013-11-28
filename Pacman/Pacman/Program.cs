using System;

namespace Pacman
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Point d’entrée principal pour l’application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Pacman game = new Pacman())
            {
                game.Run();
            }
        }
    }
#endif
}

