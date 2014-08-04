using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public MainWindow() {
            InitializeComponent();
            var xml = loadCanvas();
            draw(xml);
        }
        
        private readonly string XML_PATH = @"..\..\Designer.xml";

        private XElement loadCanvas() {
            var xml = XElement.Load(XML_PATH);
            this.mappings = xml.Element("ColorMappings").Elements().Select(i => ColorMapping.FromXml(i)).ToList();
            this.canvas.ArrayWidth = int.Parse(xml.Attribute("ArrayWidth").Value);
            this.canvas.ArrayHeight = int.Parse(xml.Attribute("ArrayHeight").Value);
            this.canvas.XMin = double.Parse(xml.Attribute("XMin").Value);
            this.canvas.XMax = double.Parse(xml.Attribute("XMax").Value);
            this.canvas.YMin = double.Parse(xml.Attribute("YMin").Value);
            this.canvas.YMax = double.Parse(xml.Attribute("YMax").Value);
            this.canvas.DefaultPixelValue = Colors.Wheat;
            this.canvas.ClearAndInitialize();
            return xml;
        }

        private List<ColorMapping> mappings;

        private void draw(XElement xml) {
            this.Progress = 0;
            int layerCount = xml.Element("Layers").Elements().Count();
            foreach (var layer in xml.Element("Layers").Elements()) {
                switch (layer.Name.LocalName) {
                    case "Function":
                        var f = FunctionLayer.FromXml(layer, this.mappings);
                        f.Paint(this.canvas);
                        break;
                    case "ConvergenceAnalysis":
                        var f2 = ConvergenceAnalysis.FromXml(layer, this.mappings);
                        f2.Paint(this.canvas);
                        break;

                }
            }
            canvas.Draw();
        }

        private void Reload_Click(object sender, RoutedEventArgs e) {
            var xml = loadCanvas();
            draw(xml);
        }

        private int _Progress;
        public int Progress {
            get { return _Progress; }
            set {
                if (value != _Progress) {
                    _Progress = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged Implementation
    }
}
