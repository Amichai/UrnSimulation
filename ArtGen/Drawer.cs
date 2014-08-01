using ArtGen.Markers;
using ArtGen.Transform;
using ArtGen.Walkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ArtGen {
    class Drawer {

        private Canvas canvas;
        IPointTransform transform;
        public Drawer(Canvas c) {
            if (c == null) {

            }
            this.canvas = c;
            this.transform = new CartesianTransformer(c.Width, c.Height);
            this.marker = new Marker(2, Brushes.Blue);
        }

        public void Add(UIElement elem) {
            this.canvas.Children.Add(elem);
        }

        public void Line(Vector p1, double angle, double l, out Vector destination) {
            double x2 = l * Math.Cos(angle) + p1.X;
            double y2 = l * Math.Sin(angle) + p1.Y;
            destination = new Vector(x2, y2);
            this.Line(p1.X, p1.Y, x2, y2);
        }

        private IMarker marker;

        public void Line(double x1, double y1, double x2, double y2) {
            var l = new Line() {
                X1 = this.transform.TransformX(x1),
                X2 = this.transform.TransformX(x2),
                Y1 = this.transform.TransformY(y1),
                Y2 = this.transform.TransformY(y2),
                StrokeThickness = marker.Thickness,
                Stroke = marker.Brush,
            };
            this.Add(l);
        }

        public void Line(Vector p1, Vector p2) {
            this.Line(p1.X, p1.Y, p2.X, p2.Y);
        }


        internal void SetMarker(IMarker marker) {
            this.marker = marker;
        }
    }
}
