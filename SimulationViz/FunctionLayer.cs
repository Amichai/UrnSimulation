using SimulationLib;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using System.Linq.Dynamic;
using System.Linq;
using System.Diagnostics;

namespace SimulationViz {
    class FunctionLayer {

        public string Function { get; private  set; }
        static Func<Vector, double> expression;
        

        internal static FunctionLayer FromXml(XElement layer) {
            var expString = layer.Attribute("FofXY").Value;
            FunctionLayer l = new FunctionLayer();
            l.Function = expString;
            expression = DynamicExpression.ParseLambda<Vector, double>(l.Function).Compile();
            return l;
        }

        public void Paint(DoubleArrayColor canvas) {
            var width = canvas.ArrayWidth;
            var height = canvas.ArrayHeight;
            var xConv = canvas.XRange / width;
            var yConv = canvas.YRange / height;
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    double x = i  * xConv + canvas.XMin;
                    double y = j * yConv + canvas.YMin;

                    byte r = (byte)((int)Math.Round(this.Eval(x, y)) % 255);
                    byte g = (byte)((int)Math.Round(this.Eval(y, x)) % 255);
                    byte b = 200;

                    canvas.PixelSet(new Vector(x, y), Color.FromRgb(r, g, b));
                }
            }
            canvas.Draw();
        }
        
        public double Eval(double x, double y) {
            return expression(new Vector(x, y));
        }
    }
}