using OxyPlot;
using OxyPlot.Series;
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

namespace LogisticMap {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            //this.model = new PlotModel();
            //this.plot.Model = model;

            int width = 1000;
            int height = 500;
            this.canvas.ArrayWidth = width;
            this.canvas.ArrayHeight = height;
            this.canvas.XMin = 2.5;
            this.canvas.YMin = 0;
            this.canvas.XMax = 4.1;
            this.canvas.YMax = 1.0;

            this.Run(1, 4, .001);
            this.canvas.Draw();
        }

        private static Random rand = new Random();

        //private PlotModel model;

        ///Test databinding the array width dp!
        //private string _PropertyName;
        //public string PropertyName {
        //    get { return _PropertyName; }
        //    set {
        //        if (_PropertyName != value) {
        //            _PropertyName = value;
        //            OnPropertyChanged("PropertyName");
        //        }
        //    }
        //}

        private List<double> sequence(Func<double, double> f, double initialVal, double maxIter) {
            List<double> toReturn = new List<double>();
            double currentVal = initialVal;
            for (int i = 0; i < maxIter; i++) {
                var newVal = f(currentVal);
                toReturn.Add(newVal);
                currentVal = newVal;
                
            }
            return toReturn.Skip(20).ToList();
        }

        public void Run(double r0, double rmax, double dr) {
            //ScatterSeries s = new ScatterSeries();
            //s.MarkerSize = .1;
            //s.MarkerStroke = OxyColors.Black;

            //int numberOfThreadsInMap = 1;
            //List<double> activeThreadValues;
            //double eps = .005;
            for (double r = r0; r < rmax; r += dr) {
                //activeThreadValues = new List<double>();
                Func<double, double> log = i => r * i * (1 - i);
                var seq = sequence(log, .2, 360);
                seq.Reverse();
                var results = seq.Take(150);
                foreach (var A in results) {
                    //if (activeThreadValues.Where(j => j < A + eps && j > A - eps).Count() == 0)
                    //    activeThreadValues.Add(A);
                    //s.Points.Add(new ScatterPoint(r, A));
                    this.canvas.PixelSet(new Vector(r, A), 255);
                }
                //if (activeThreadValues.Count > numberOfThreadsInMap) {
                //    Console.WriteLine("Bifurcation point: " + r.ToString() + " " + activeThreadValues.Count.ToString() + " threads.");
                //    numberOfThreadsInMap = activeThreadValues.Count();
                //    eps /= numberOfThreadsInMap;
                //}
            }
            //this.model.Series.Add(s);
            //this.model.InvalidatePlot(true);
        }
    }
}
