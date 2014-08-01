using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArtGen.Transform {
    class CartesianTransformer : IPointTransform {
        private double width, height;
        public CartesianTransformer(double width, double height) {
            this.width = width;
            this.height = height;
        }
        public double TransformX(double x) {
            return x + this.width / 2.0;
        }

        public double TransformY(double y) {
            return this.height - (y + this.height / 2.0);
        }

        public Vector Transform(Vector pt) {
            var x = TransformX(pt.X);
            var y = TransformY(pt.Y);
            return new Vector(x, y);
        }
    }
}
