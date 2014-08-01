using ArtGen.Markers;
using ArtGen.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ArtGen.Walkers {
    class RandomWalker : IWalker {
        private static IRandom rand = new EvenDistribution();
        private IMarker marker;
        private Canvas canvas;
        private double stepSize;
        private Drawer d;
        public RandomWalker(Canvas c, double stepSize, Vector initialPosition) {
            this.canvas = c;
            d = new Drawer(this.canvas);
            this.stepSize = stepSize;
            this.pos = initialPosition;
        }

        public void SetMarker(IMarker marker) {
            this.marker = marker;
            this.d.SetMarker(marker);
        }
        private Vector pos;

        public void Walk() {
            var l = rand.Next() * this.stepSize;
            var angle = rand.Next() * Math.PI * 2;
            
            d.Line(pos, angle, l, out pos);
        }

        public void Walk(double dist) {
            throw new NotImplementedException();
        }
    }
}
