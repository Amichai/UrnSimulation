using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IsingModel {
    class Lattice2D {
        public Lattice2D(int width, int height, double positiveSpinProbability) {
            this.positiveSpinProbability = positiveSpinProbability;
            this.Width = width;
            this.Height = height;
            this.initialize();
        }

        private static Random rand = new Random();

        public void SetTemperature(double t) {
            this.T = t;
        }

        private double? positiveSpinProbability;

        /// <summary>
        /// initialize our lattice with randomly configured valuse of +1 and -1
        /// </summary>
        private void initialize() {
            this.T = 1;
            this.SpinSum = 0;
            this.lattice = new int[this.Width][];
            for (int i = 0; i < this.Width; i++) {
                this.lattice[i] = new int[this.Height];
            }

            for (int i = 0; i < this.Width; i++) {
                for (int j = 0; j < this.Height; j++) {
                    int val = rand.NextDouble() < this.positiveSpinProbability.Value ? 1 : -1;
                    this.SpinSum += val;
                    this.lattice[i][j] = val;
                }
            }
        }

        /// <summary>
        /// H =  -J * sum over all adjacent cell[spin1 * spin2]
        /// H is the hamiltonian (energy of the system)
        /// J is the interaction energy between spins
        /// </summary>
        public double Energy() {
            double energySum = 0;
            for (int j = 0; j < this.Height; j++) {
                for (int i = 0; i < this.Width; i++) {
                    int spin1 = this.lattice[i][j];
                    int spin2 = this.lattice[(i + 1) % this.Width][j];
                    energySum +=  -1 * this.interactionEnergy * spin1 * spin2;
                }
            }
            for (int i = 0; i < this.Width; i++) {
                for (int j = 0; j < this.Height; j++) {
                    int spin1 = this.lattice[i][j];
                    int spin2 = this.lattice[i][(j + 1) % this.Height];
                    energySum += -1 * this.interactionEnergy * spin1 * spin2;
                }
            }
            return energySum;
        }

        private double interactionEnergy = 1;

        public int Width { get; private set; }
        public int Height { get; private set; }

        private int[][] lattice;
        public int Get(int x, int y) {
            if (x < 0) {
                x += this.Width;
            }
            if (y < 0) {
                y += this.Height;
            }
            return this.lattice[x % this.Width][y % this.Height];
        }

        public double T { get; private set; }
        

        private int surroundingOrientation(int x, int y) {
            return this.Get(x + 1, y) + this.Get(x - 1, y) + this.Get(x, y + 1) + this.Get(x, y - 1);
        }

        private int flip(int x, int y) {
            this.lattice[x % this.Width][y % this.Height] = this.lattice[x % this.Width][y % this.Height] * -1;
            var v = this.lattice[x % this.Width][y % this.Height];
            if (v == 1) {
                SpinSum += 2;
            } else {
                SpinSum -= 2;
            }
            return v;
        }

        internal Vector Perturb() {
            int x = rand.Next(this.Width);
            int y = rand.Next(this.Height);
            Vector p = new Vector(x, y);
            var surrounding = this.surroundingOrientation(x, y);
            ///The energy goes down if the spins are more aligned 

            int changeInEnergy;
            int inspection = this.Get(x, y);
            if (Math.Sign(inspection) == Math.Sign(surrounding)) {
                changeInEnergy = Math.Abs(surrounding * 2);
            } else {
                changeInEnergy = -Math.Abs(surrounding * 2);
            }
            if (changeInEnergy <= 0) {
                this.flip(x, y);
            } else {
                double prob = Math.Exp(-(1 / T) * changeInEnergy);
                if (rand.NextDouble() < prob) {
                    this.flip(x, y);
                } else {
                }
            }
            return p;
        }

        public int SpinSum { get; private set; }
    }
}
