using SimulationLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace SimulationViz {
    class ConvergenceAnalysis {
        internal static ConvergenceAnalysis FromXml(XElement layer, List<ColorMapping> list) {


            return new ConvergenceAnalysis();
        }

        internal void Paint(DoubleArrayColor canvas) {
            this.Run(1, 4, .001, canvas);

        }


        private List<double> sequence(Func<double, double> f, double initialVal, double maxIter) {
            List<double> toReturn = new List<double>();
            double currentVal = initialVal;
            for (int i = 0; i < maxIter; i++) {
                var newVal = f(currentVal);
                toReturn.Add(newVal);
                currentVal = newVal;

            }
            return toReturn.Skip(20).ToList();
        }

        public void Run(double r0, double rmax, double dr, DoubleArrayColor canvas) {
            for (double r = r0; r < rmax; r += dr) {
                Func<double, double> log = i => r * i * (1 - i);
                var seq = sequence(log, .2, 360);
                seq.Reverse();
                var results = seq.Take(150);
                foreach (var A in results) {
                    canvas.PixelSet(new Vector(r, A), Colors.Black);
                }
            }
        }
    }
}
