﻿using ArtGen.Walkers;
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

namespace ArtGen {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            var d = new Drawer(this.canvas);
            d.Line(20, 20, 50, 50);
            var w = new RandomWalker(this.canvas, 10, new Vector(0,0));
            w.SetMarker(new Marker(.5, Brushes.Green));
            for (int i = 0; i < 100; i++) {
                w.Walk();
            }
        }
    }
}
