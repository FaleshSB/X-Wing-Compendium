using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    public static class Rng
    {
        private static Random random = new Random();

        public static int Next()
        {
            return random.Next();
        }
        public static int Next(int max)
        {
            return random.Next(max);
        }
        public static int Next(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
