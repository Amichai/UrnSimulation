using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace UrnVisualizer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public MainWindow() {
            InitializeComponent();
            this.NumberOfDraws = 1000;
            this.NumberOfTrials = 20;
            this.plotModel = new PlotModel();
            this.plot.Model = this.plotModel;
        }
        
        private PlotModel plotModel;

        
        private int _NumberOfDraws;
        public int NumberOfDraws {
            get { return _NumberOfDraws; }
            set { 
                _NumberOfDraws = value;
                OnPropertyChanged("NumberOfDraws");
            }
        }

        private int _NumberOfTrials;
        public int NumberOfTrials {
            get { return _NumberOfTrials; }
            set { 
                _NumberOfTrials = value;
                OnPropertyChanged("NumberOfTrials");
            }
        }
        
        

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) {
            var eh = PropertyChanged;
            if (eh != null) {
                eh(this, new PropertyChangedEventArgs(name));
            }
        }

        private static Random rand = new Random();

        private void Spawn_Click(object sender, RoutedEventArgs e) {
            for (int i = 0; i < this.NumberOfTrials; i++) {
                ///Spawn a trial
                var u = new Urn();
                LineSeries series = new LineSeries();
                
                for (int j = 0; j < this.NumberOfDraws; j++) {
                    var p = u.Draw();
                    DataPoint pt = new DataPoint(j, p);
                    series.Points.Add(pt);
                }

                this.plotModel.Series.Add(series);
                ///Plot the result of the trial
            }
            this.plotModel.InvalidatePlot(true);
        }
    }
}
