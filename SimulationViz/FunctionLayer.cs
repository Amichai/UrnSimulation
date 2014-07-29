using SimulationLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SimulationViz {
    class FunctionLayer {

        public string Function { get; private  set; }
        internal static FunctionLayer FromXml(XElement layer) {
            

            var expString = layer.Attribute("FofXY").Value;
            FunctionLayer l = new FunctionLayer();
            l.Function = expString;
            return l;
        }

        public void Paint(DoubleArray canvas) {
            var width = canvas.ArrayWidth;
            var height = canvas.ArrayHeight;
            var xConv = canvas.XRange / width;
            var yConv = canvas.YRange / height;
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    double x = i  * xConv + canvas.XMin;
                    double y = j * yConv + canvas.YMin;
                    byte v = (byte)((int)Math.Round(this.Eval(x, y)) % 255);
                    canvas.PixelSet(new Vector(x, y), v);
                }
            }
            canvas.Draw();
        }

        public double Eval(double x, double y) {
            var e = IronPython.Hosting.Python.CreateEngine();
            var scope = e.CreateScope();
            scope.SetVariable("x", x);
            scope.SetVariable("y", y);
            var r = e.Execute(this.Function, scope);
            return r;
        }
    }
}
