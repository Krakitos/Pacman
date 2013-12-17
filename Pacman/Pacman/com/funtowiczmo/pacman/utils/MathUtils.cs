using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.utils
{
    public class MathUtils
    {
        private static Random random = new Random();

        public static int Random()
        {
            return random.Next();
        }

        public static int Random(int maxVal)
        {
            return random.Next(maxVal);
        }

        public static int Random(int minVal, int maxVal)
        {
            return random.Next(minVal, maxVal);
        }
    }
}
