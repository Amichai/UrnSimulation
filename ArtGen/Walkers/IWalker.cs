using ArtGen.Markers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGen.Walkers {
    interface IWalker {
        void Walk();
        void Walk(double dist);
        void SetMarker(IMarker marker);
    }
}
