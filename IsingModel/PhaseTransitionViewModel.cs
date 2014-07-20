using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IsingModel {
    public class PhaseTransitionViewModel : INotifyPropertyChanged {
        public PhaseTransitionViewModel() {
            this.PlotModel = new PlotModel();
            this.series = new LineSeries();
            
            this.T0 = 0;
            this.Tf = 4;
            this.dT = .2;
            this.Iter = 1000000;
            this.RunSimulation();
        }

        private LineSeries series;

        private PlotModel _PlotModel;
        public PlotModel PlotModel {
            get { return _PlotModel; }
            set {
                _PlotModel = value;
                NotifyPropertyChanged();
            }
        }

        private int _Iter;
        public int Iter {
            get { return _Iter; }
            set {
                _Iter = value;
                NotifyPropertyChanged();
            }
        }

        private double _T0;
        public double T0 {
            get { return _T0; }
            set {
                _T0 = value;
                NotifyPropertyChanged();
            }
        }

        private double _Tf;
        public double Tf {
            get { return _Tf; }
            set {
                _Tf = value;
                NotifyPropertyChanged();
            }
        }

        private double _dT;
        public double dT {
            get { return _dT; }
            set {
                _dT = value;
                NotifyPropertyChanged();
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

        private int width = 1000, height = 1000;

        internal void RunSimulation() {
            this.PlotModel.Series.Clear();
            this.series = new LineSeries();
            this.PlotModel.Series.Add(series);
            for (double t = this.T0; t < this.Tf; t += dT) {
                var l = new Lattice2D(width, height, .5);
                for (int i = 0; i < this.Iter; i++) {
                    l.Perturb();
                }
                this.series.Points.Add(new DataPoint(t, l.SpinSum));
            }
            this.PlotModel.InvalidatePlot(true);
        }
    }
}
