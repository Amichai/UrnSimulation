using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace ChaosGame {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public MainWindow() {
            InitializeComponent();
            this.Radius = 200;

            this.initialVertices = new List<Vector>();
            this.NumberOfVertices = 4;
            this.NumberOfIterations = 100000;
            this.FractionalStep = .6;
        }

        private int _ProgressVal;
        public int ProgressVal {
            get { return _ProgressVal; }
            set {
                if (_ProgressVal != value) {
                    _ProgressVal = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _NumberOfIterations;
        public int NumberOfIterations {
            get { return _NumberOfIterations; }
            set {
                _NumberOfIterations = value;
                NotifyPropertyChanged();
            }
        }

        private double _FractionalStep;
        public double FractionalStep {
            get { return _FractionalStep; }
            set {
                _FractionalStep = value;
                NotifyPropertyChanged();
            }
        }

        private int _NumberOfVertices;
        public int NumberOfVertices {
            get { return _NumberOfVertices; }
            set {
                _NumberOfVertices = value;
                NotifyPropertyChanged();
            }
        }

        private List<Vector> initialVertices;
        private Vector currentPosition;

        private void reset() {
            //this.canvas.Children.Clear();
            this.canvas.Clear();
        }

        private List<Vector> getInitialVertices(int numberOfVertices) {
            List<Vector> outputVectors = new List<Vector>();
            double theta = (Math.PI * 2) / numberOfVertices;
            for (int i = 0; i < numberOfVertices; i++) {
                double x = this.Radius * Math.Cos(theta * i);
                double y = this.Radius * Math.Sin(theta * i);
                Vector v = new Vector(x, y);
                outputVectors.Add(v);
            }
            return outputVectors;
        }

        private void draw(Vector v) {
            //Ellipse e = new Ellipse() {
            //    Width = 1,
            //    Height = 1,
            //    Fill = Brushes.Black,  
            //};
            //this.canvas.Children.Add(e);
            var x = v.X + this.Radius;
            var y = v.Y + this.Radius;
            //Debug.Print(string.Format("{0}, {1}", x, y));

            Dispatcher.Invoke((Action)(() => {
                this.canvas.PixelSet(v, 0);

            }));
            //Canvas.SetLeft(e, x);
            //Canvas.SetTop(e, y);
        }

        private static Random rand = new Random();

        private Vector getRandomPosition(double maxVal) {
            double x = rand.NextDouble() * maxVal;
            double y = rand.NextDouble() * maxVal;
            return new Vector(x, y);
        }

        private Vector pickRandomVertex() {
            int idx = rand.Next(this.NumberOfVertices);
            return this.initialVertices[idx];
        }

        private double _Radius;
        public double Radius {
            get { return _Radius; }
            set {
                if (_Radius != value) {
                    _Radius = value;

                    this.canvas.XMin = -value;
                    this.canvas.XMax = value;
                    this.canvas.YMin = -value;
                    this.canvas.YMax = value;
                    this.canvas.ArrayWidth = (int)Math.Round(value * 2);
                    this.canvas.ArrayHeight = (int)Math.Round(value * 2);

                    this.canvas.Clear();
                    NotifyPropertyChanged("Radius");
                }
            }
        }

        private void iterate() {
            var targetVertex = pickRandomVertex();
            var dx = (targetVertex.X - currentPosition.X) * this.FractionalStep;
            var dy = (targetVertex.Y - currentPosition.Y) * this.FractionalStep;
            double newX = currentPosition.X + dx;
            double newY = currentPosition.Y + dy;
            this.currentPosition = new Vector(newX, newY);
            this.draw(this.currentPosition);
        }

        private void Run_Click(object sender, RoutedEventArgs e) {
            this.reset();
            this.initialVertices = this.getInitialVertices(this.NumberOfVertices);
            this.currentPosition = getRandomPosition(this.Radius);
            Task.Run(() => {
                for (int i = 0; i < this.NumberOfIterations; i++) {
                    iterate();
                    Dispatcher.Invoke((Action)(() => {

                        this.ProgressVal = i * 100 / this.NumberOfIterations;
                    }));
                }
                    Dispatcher.Invoke((Action)(() => {

                this.ProgressVal = 0;
                    }));
                Dispatcher.Invoke((Action)(() => {
                    this.canvas.Draw();
                }));
            });
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
