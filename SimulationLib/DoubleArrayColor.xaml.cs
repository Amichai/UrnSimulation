using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

namespace SimulationLib {
    /// <summary>
    /// Interaction logic for DoubleArrayColor.xaml
    /// </summary>
    public partial class DoubleArrayColor : UserControl, INotifyPropertyChanged, IDraw {
        public DoubleArrayColor() {
            InitializeComponent();
            this.ContentRendered = false;
        }

        /// <summary>
        /// Must be called once before calling setPixel()
        /// </summary>
        public void ClearAndInitialize() {
            this.init();
            this.ContentRendered = false;
        }

        private DoubleArray r = new DoubleArray();
        private DoubleArray g = new DoubleArray();
        private DoubleArray b = new DoubleArray();
        

        #region Dependency Properties



        private double _YMax;
        public double YMax {
            get {
                return _YMax;
            }
            set {
                _YMax = value;
                this.r.YMax = value;
                this.g.YMax = value;
                this.b.YMax = value;
            }
        }

        private double _XMax;
        public double XMax {
            get { return _XMax; }
            set { 
                _XMax = value;
                this.r.XMax = value;
                this.g.XMax = value;
                this.b.XMax = value;
            }
        }



        private double _XMin;
        public double XMin {
            get { return _XMin; }
            set {
                _XMin = value;
                this.r.XMin = value;
                this.g.XMin = value;
                this.b.XMin = value;
            }
        }

        private double _YMin;
        public double YMin {
            get { return _YMin; }
            set {
                _YMin = value;
                this.r.YMin = value;
                this.g.YMin = value;
                this.b.YMin = value;
            }
        }


        private int _ArrayWidth;
        public int ArrayWidth {
            get { return _ArrayWidth; }
            set { 
                _ArrayWidth = value;
                this.r.ArrayWidth = value;
                this.g.ArrayWidth = value;
                this.b.ArrayWidth = value;
            }
        }

        private int _ArrayHeight;
        public int ArrayHeight {
            get { return _ArrayHeight; }
            set {
                _ArrayHeight = value;
                this.r.ArrayHeight = value;
                this.g.ArrayHeight = value;
                this.b.ArrayHeight = value;
            }
        }
        
        public byte DefaultPixelValue {
            get { return (byte)GetValue(DefaultPixelValueProperty); }
            set { SetValue(DefaultPixelValueProperty, value); }
        }

        public static readonly DependencyProperty DefaultPixelValueProperty =
            DependencyProperty.Register("DefaultPixelValue", typeof(byte), typeof(DoubleArrayColor), new PropertyMetadata((byte)0));
        #endregion

        public double XRange {
            get {
                return this.XMax - this.XMin;
            }
        }

        public double YRange {
            get {
                return this.YMax - this.YMin;
            }
        }

        private ImageSource _ImageSource;
        public ImageSource ImageSource {
            get { return _ImageSource; }
            set {
                _ImageSource = value;
                NotifyPropertyChanged("ImageSource");
            }
        }

        public void Draw() {
            if (this.XRange <= 0) {
                return;
            }
            if (this.YRange <= 0) {
                return;
            }
            this.ImageSource = toImageSource();
            this.ContentRendered = true;
        }

        private void init() {
            this.r.ClearAndInitialize();
            this.g.ClearAndInitialize();
            this.b.ClearAndInitialize();
        }

        public bool ContentRendered { get; private set; }

        public bool AreAxesVisible {
            get {
                return this.ShowAxesLabels && this.ContentRendered;
            }
        }

        private bool _ShowAxesLabels;
        public bool ShowAxesLabels {
            get { return _ShowAxesLabels; }
            set {
                _ShowAxesLabels = value;
                NotifyPropertyChanged("ShowAxesLabels");
                NotifyPropertyChanged("AreAxesVisible");
            }
        }

        //private byte[][] frame;

        public void PixelAdd(Vector v, Color val) {
            var i = rd(((v.X - this.XMin) / this.XRange) * this.ArrayWidth);
            var j = rd(((v.Y - this.YMin) / this.YRange) * this.ArrayHeight);
            if (i < 0 || i > this.ArrayWidth - 1) {
                return;
            }
            if (j < 0 || j > this.ArrayHeight - 1) {
                return;
            }

            var y = this.ArrayHeight - j - 1;

            this.r.AddToCell(i, y, val.R);
            this.g.AddToCell(i, y, val.G);
            this.b.AddToCell(i, y, val.B);
        }

        /// <summary>
        /// Be sure to call clearAndInitialize() before setting pixels on the canvas
        /// </summary>
        public void PixelSet(Vector v, Color val) {
            var i = rd(((v.X - this.XMin) / this.XRange) * this.ArrayWidth);
            var j = rd(((v.Y - this.YMin) / this.YRange) * this.ArrayHeight);
            if (i < 0 || i > this.ArrayWidth - 1) {
                return;
            }
            if (j < 0 || j > this.ArrayHeight - 1) {
                return;
            }

            var y = this.ArrayHeight - j - 1;

            this.r.SetCell(i, y, val.R);
            this.g.SetCell(i, y, val.G);
            this.b.SetCell(i, y, val.B);
        }

        private int rd(double r) {
            return (int)Math.Round(r);
        }

        private byte[] toByteArray() {
            var count = this.ArrayWidth * this.ArrayHeight * 3;
            byte[] output = new byte[count];
            int counter = 0;
            for (int i = 0; i < this.ArrayHeight; i++) {
                for (int j = 0; j < this.ArrayWidth; j++) {
                    output[counter++] = this.r.GetCell(j, i);
                    output[counter++] = this.g.GetCell(j, i);
                    output[counter++] = this.b.GetCell(j, i);
                }
            }
            return output;
        }

        public BitmapSource toImageSource() {
            var buffer = toByteArray();
            var dpiX = 96d;
            var dpiY = 96d;
            var pixelFormat = PixelFormats.Rgb24; // Color bitmap
            var bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8; // == 3 in this example

            var stride = bytesPerPixel * this.ArrayWidth; // == width in this example
            var bitmap = BitmapSource.Create(this.ArrayWidth, this.ArrayHeight, dpiX, dpiY,
                                             pixelFormat, null, buffer, stride);

            return bitmap as BitmapSource;
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged Implementation

        private void SaveClick_1(object sender, RoutedEventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = ".png";
            sfd.ShowDialog();
            var filename = sfd.FileName;
            if (string.IsNullOrWhiteSpace(filename)) {
                return;
            }
            this.Save(filename);
            Process.Start(filename);
        }

        /// <summary>
        /// Will only work after the consumer calls the Draw() method
        /// </summary>
        public void Save(string filePath) {
            using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                BitmapEncoder encoder = new PngBitmapEncoder();
                var a = BitmapFrame.Create(this.ImageSource as BitmapSource);
                encoder.Frames.Add(a);
                encoder.Save(fileStream);
            }
        }

    }
}
