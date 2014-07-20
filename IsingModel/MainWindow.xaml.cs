using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace IsingModel {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public MainWindow() {
            InitializeComponent();

            this.LatticeSimulation = new LatticeSimulationViewModel(this.canvas, Dispatcher);
            this.PhaseTransition = new PhaseTransitionViewModel();
        }

        private LatticeSimulationViewModel _LatticeSimulation;
        public LatticeSimulationViewModel LatticeSimulation {
            get { return _LatticeSimulation; }
            set {
                _LatticeSimulation = value;
                NotifyPropertyChanged();
            }
        }

        private PhaseTransitionViewModel _PhaseTransition;
        public PhaseTransitionViewModel PhaseTransition {
            get { return _PhaseTransition; }
            set {
                _PhaseTransition = value;
                NotifyPropertyChanged();
            }
        }

        private void Reset_Click_1(object sender, RoutedEventArgs e) {
            this.LatticeSimulation.Reset();
        }

        private void StartStop_Click_1(object sender, RoutedEventArgs e) {
            this.LatticeSimulation.StartStop();
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged Implementation

        private void RunSimulation_Click_1(object sender, RoutedEventArgs e) {
            this.PhaseTransition.RunSimulation();
        }
    }
}
