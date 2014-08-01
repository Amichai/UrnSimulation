using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArtGen {
    interface IPointTransform {
        double TransformX(double x);
        double TransformY(double y);
        Vector Transform(Vector pt);
    }
}
