using ArtGen.Markers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ArtGen {
    class Marker : IMarker {
        public Marker(double thickness, Brush brush) {
            this.Thickness = thickness;
            this.Brush = brush;
        }
        public double Thickness { get; set; }
        public Brush Brush { get; set; }

    }
}
