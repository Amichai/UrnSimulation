using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SimulationViz {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            var xml = XElement.Load(@"Designer.xml");
            this.canvas.ArrayWidth = int.Parse(xml.Attribute("ArrayWidth").Value);
            this.canvas.ArrayHeight = int.Parse(xml.Attribute("ArrayHeight").Value);
            this.canvas.XMin = double.Parse(xml.Attribute("XMin").Value);
            this.canvas.XMax = double.Parse(xml.Attribute("XMax").Value);
            this.canvas.YMin = double.Parse(xml.Attribute("YMin").Value);
            this.canvas.YMax = double.Parse(xml.Attribute("YMax").Value);


            foreach (var layer in xml.Element("Layers").Elements()) {
                switch (layer.Name.LocalName) {
                    case "Function":
                        var f = FunctionLayer.FromXml(layer);
                        f.Paint(this.canvas);
                        break;

                }
            }
        }
    }
}
