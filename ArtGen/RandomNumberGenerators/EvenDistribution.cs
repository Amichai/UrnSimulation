using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGen.RandomNumberGenerators {
    class EvenDistribution : IRandom {
        private static Random rand = new Random();
        public double Next() {
            return rand.NextDouble();
        }
    }
}
