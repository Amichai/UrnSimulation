using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrnSimulation {
    public class Urn {
        public Urn() {
            this.WhiteBalls = 1;
            this.BlackBalls = 1;
        }

        public int WhiteBalls { get; set; }
        public int BlackBalls { get; set; }

        private static Random rand = new Random();

        public int TotalNumberOfBalls {
            get {
                return WhiteBalls + BlackBalls;
            }
        }

        /// <summary>Picks a ball at random from the urn</summary>
        /// <returns>The ratio of white balls in the urn</returns>
        public double Draw() {
            var randomIndex = rand.Next(TotalNumberOfBalls);
            if (randomIndex < WhiteBalls) {
                ///A white ball was picked
                this.WhiteBalls++;
            } else {
                ///A black ball was picked
                this.BlackBalls++;
            }
            return this.WhiteBalls / (double)this.TotalNumberOfBalls;
        }
    }
}
