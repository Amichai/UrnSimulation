using System;
using System.Collections.Generic;
//using System.Linq;
using System.Linq.Dynamic;
//using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace SimulationViz {
    class ColorMapping {

        Func<double?, double> d1;
        Func<double?, double> d2;
        Func<double?, double> d3;

        public string Name { get; private set; }
        public static ColorMapping FromXml(XElement xml) {
            var m = new ColorMapping();
            var r = xml.Attribute("R").Value;
            var g = xml.Attribute("G").Value;
            var b = xml.Attribute("B").Value;
            m.Name = xml.Attribute("Name").Value;
            double Value = 4;
            var c = (byte)((int)Math.Round(Value) % 255);

            m.d1 = DynamicExpression.ParseLambda<double?, double>(r).Compile();
            m.d2 = DynamicExpression.ParseLambda<double?, double>(g).Compile();
            m.d3 = DynamicExpression.ParseLambda<double?, double>(b).Compile();


            return m;
        }

        byte toByte(double d) {
            return (byte)Math.Round(d);
        }

        internal Color Map(double p) {
            byte r = toByte(d1(p));
            byte g = toByte(d2(p));
            byte b = toByte(d3(p));

            return Color.FromRgb(r, g, b);
        }
    }
}
