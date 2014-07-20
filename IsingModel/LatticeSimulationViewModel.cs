using OxyPlot;
using OxyPlot.Series;
using SimulationLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace IsingModel {
    public class LatticeSimulationViewModel : INotifyPropertyChanged {
        public LatticeSimulationViewModel(DoubleArray c, Dispatcher dispatcher) {
            this.canvas = c;
            this.currentDispatcher = dispatcher;
            this.canvas.Setup(width, height, 0, width, 0, height);

            this.reset(width, height);
            this.Temperature = 3;
            this.TimeStep = 1;
        }
        private DoubleArray canvas;
        private Dispatcher currentDispatcher;

        private void Run() {
            Task.Run(() => {
                while (true) {
                    if (!running) {
                        return;
                    }
                    var p = lattice.Perturb();

                    try {
                        var e = lattice.Energy();
                        this.currentDispatcher.Invoke((Action)(() => {
                            var v = getPixelByte(lattice, p);
                            this.canvas.PixelSet(p, v);
                            series.Points.Add(new DataPoint(t, e));
                            if (series.Points.Count() > 1000) {
                                series.Points.RemoveAt(0);
                            }
                            if (t % TimeStep == 0) {
                                this.Energy = e;
                                this.SpinSum = this.lattice.SpinSum;
                                this.canvas.Draw();
                                Thread.Sleep(1);
                                this.PlotModel.InvalidatePlot(true);
                            }
                        }));
                    } catch { }
                    t++;
                }
            });
        }

        private void reset(int width, int height) {
            this.t = 0;

            this.lattice = new Lattice2D(width, height, .5);
            this.PlotModel = new PlotModel();
            this.series = new LineSeries();
            this.PlotModel.Series.Add(series);
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    var p = new Vector(i, j);
                    var v = getPixelByte(lattice, p);
                    this.canvas.PixelSet(p, v);
                }
            }
        }

        private static byte getPixelByte(Lattice2D l, Vector p) {
            var v = (byte)(l.Get((int)p.X, (int)p.Y) * 100 + 100);
            return v;
        }

        private int width = 100, height = 100;
        private int t;
        private LineSeries series;
        private Lattice2D lattice;

        private PlotModel _PlotModel;
        public PlotModel PlotModel {
            get { return _PlotModel; }
            set {
                _PlotModel = value;
                NotifyPropertyChanged();
            }
        }

        private double _Temperature;
        public double Temperature {
            get { return _Temperature; }
            set {
                _Temperature = value;
                this.lattice.SetTemperature(value);
                NotifyPropertyChanged();
            }
        }

        private int _TimeStep;
        public int TimeStep {
            get { return _TimeStep; }
            set {
                _TimeStep = value;
                NotifyPropertyChanged();
            }
        }

        private double _Energy;
        public double Energy {
            get { return _Energy; }
            set {
                _Energy = value;
                this.NormalizedEnergy = value / (double)(this.width * this.height);
                NotifyPropertyChanged();
            }
        }

        private int _SpinSum;
        public int SpinSum {
            get { return _SpinSum; }
            set {
                _SpinSum = value;
                this.NormalizedSpinSum = value / (double)(this.width * this.height);
                NotifyPropertyChanged();
            }
        }

        private double _NormalizedSpinSum;
        public double NormalizedSpinSum {
            get { return _NormalizedSpinSum; }
            set {
                _NormalizedSpinSum = value;
                NotifyPropertyChanged();
            }
        }

        private double _NormalizedEnergy;
        public double NormalizedEnergy {
            get { return _NormalizedEnergy; }
            set {
                _NormalizedEnergy = value;
                NotifyPropertyChanged();
            }
        }


        private bool running = false;

        public string StartStopText {
            get {
                if (!running) {
                    return "Start";
                } else {
                    return "Stop";
                }
            }
        }

        public void Reset() {
            this.reset(width, height);
        }

        public void StartStop() {
            this.running = !this.running;
            NotifyPropertyChanged("StartStopText");
            if (this.running) {
                this.Run();
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
