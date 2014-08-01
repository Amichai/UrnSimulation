using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ArtGen.Markers {
    interface IMarker {
        double Thickness { get; set; }
        Brush Brush { get; set; }
    }
}
