using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public MainWindow() {
            InitializeComponent();
            this.canvas.ShowAxesLabels = true;
            this.Resolution = 500;
            this.XMin = 1;
            this.YMin = 0;
            this.XMax = 4.0;
            this.YMax = 1.0;
        }
        
        private static Random rand = new Random();

        private double _XMin;
        public double XMin {
            get { return _XMin; }
            set {
                _XMin = value;
                this.canvas.XMin = value;
                NotifyPropertyChanged("XMin");
            }
        }

        private double _XMax;
        public double XMax {
            get { return _XMax; }
            set {
                _XMax = value;
                this.canvas.XMax = value;
                NotifyPropertyChanged("XMax");
            }
        }

        private double _YMin;
        public double YMin {
            get { return _YMin; }
            set {
                _YMin = value;
                this.canvas.YMin = value;
                NotifyPropertyChanged("YMin");
            }
        }

        private double _YMax;
        public double YMax {
            get { return _YMax; }
            set {
                _YMax = value;
                this.canvas.YMax = value;
                NotifyPropertyChanged("YMax");
            }
        }

        private int _Resolution;
        public int Resolution {
            get { return _Resolution; }
            set {
                _Resolution = value;
                NotifyPropertyChanged("Resolution");
            }
        }

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
                    this.canvas.PixelSet(new Vector(r, A), 0);
                }
                //if (activeThreadValues.Count > numberOfThreadsInMap) {
                //    Console.WriteLine("Bifurcation point: " + r.ToString() + " " + activeThreadValues.Count.ToString() + " threads.");
                //    numberOfThreadsInMap = activeThreadValues.Count();
                //    eps /= numberOfThreadsInMap;
                //}
            }
        }

        private void Draw_Click_1(object sender, RoutedEventArgs e) {
            this.canvas.ArrayWidth = (int)Math.Round(this.canvas.XRange * this.Resolution);
            this.canvas.ArrayHeight = (int)Math.Round(this.canvas.YRange * this.Resolution);

            this.canvas.ClearAndInitialize();
            this.Run(1, 4, .001);
            this.canvas.Draw();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
