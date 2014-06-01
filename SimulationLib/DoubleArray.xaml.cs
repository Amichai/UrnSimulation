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
    /// Interaction logic for DoubleArray.xaml
    /// </summary>
    public partial class DoubleArray : UserControl, INotifyPropertyChanged, IDraw {
        public DoubleArray() {
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

        #region Dependency Properties
        public double XMin {
            get { return (double)GetValue(XMinProperty); }
            set { SetValue(XMinProperty, value); }
        }

        public static readonly DependencyProperty XMinProperty =
            DependencyProperty.Register("XMin", typeof(double), typeof(DoubleArray), new PropertyMetadata(0.0));

        public double XMax {
            get { return (double)GetValue(XMaxProperty); }
            set { SetValue(XMaxProperty, value); }
        }

        public static readonly DependencyProperty XMaxProperty =
            DependencyProperty.Register("XMax", typeof(double), typeof(DoubleArray), new PropertyMetadata(0.0));

        public double YMin {
            get { return (double)GetValue(YMinProperty); }
            set { SetValue(YMinProperty, value); }
        }

        public static readonly DependencyProperty YMinProperty =
            DependencyProperty.Register("YMin", typeof(double), typeof(DoubleArray), new PropertyMetadata(0.0));

        public double YMax {
            get { return (double)GetValue(YMaxProperty); }
            set { SetValue(YMaxProperty, value); }
        }

        public static readonly DependencyProperty YMaxProperty =
            DependencyProperty.Register("YMax", typeof(double), typeof(DoubleArray), new PropertyMetadata(0.0));

        public int ArrayWidth {
            get { return (int)GetValue(ArrayWidthProperty); }
            set { SetValue(ArrayWidthProperty, value); }
        }

        public static readonly DependencyProperty ArrayWidthProperty =
            DependencyProperty.Register("ArrayWidth", typeof(int), typeof(DoubleArray), new PropertyMetadata(0));

        public int ArrayHeight {
            get { return (int)GetValue(ArrayHeightProperty); }
            set { SetValue(ArrayHeightProperty, value); }
        }

        public static readonly DependencyProperty ArrayHeightProperty =
            DependencyProperty.Register("ArrayHeight", typeof(int), typeof(DoubleArray), new PropertyMetadata(0));

        public byte DefaultPixelValue {
            get { return (byte)GetValue(DefaultPixelValueProperty); }
            set { SetValue(DefaultPixelValueProperty, value); }
        }

        public static readonly DependencyProperty DefaultPixelValueProperty =
            DependencyProperty.Register("DefaultPixelValue", typeof(byte), typeof(DoubleArray), new PropertyMetadata((byte)0));
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
            this.frame = new byte[this.ArrayWidth][];
            for (int i = 0; i < this.ArrayWidth; i++) {
                frame[i] = new byte[this.ArrayHeight];
                for (int j = 0; j < this.ArrayHeight; j++) {
                    frame[i][j] = this.DefaultPixelValue;
                }
            }
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

        private byte[][] frame;

        /// <summary>
        /// Be sure to call clearAndInitialize() before setting pixels on the canvas
        /// </summary>
        public void PixelSet(Vector v, byte val) {
            if (this.frame == null) {
                this.init();
            }
            var i = rd(((v.X - this.XMin) / this.XRange) * this.ArrayWidth);
            var j = rd(((v.Y - this.YMin) / this.YRange) * this.ArrayHeight);
            if (i < 0 || i > this.ArrayWidth - 1) {
                return;
            }
            if (j < 0 || j > this.ArrayHeight - 1) {
                return;
            }
            this.frame[i][this.ArrayHeight - j - 1] = val;
        }

        private int rd(double r) {
            return (int)Math.Round(r);
        }

        private byte[] toByteArray() {
            var count = this.ArrayWidth * this.ArrayHeight;
            byte[] output = new byte[count];
            int counter = 0;
            for (int i = 0; i < this.ArrayHeight; i++) {
                for (int j = 0; j < this.ArrayWidth; j++) {
                    output[counter++] = this.frame[j][i];
                }
            }
            return output;
        }

        public BitmapSource toImageSource() {
            var buffer = toByteArray();
            var dpiX = 96d;
            var dpiY = 96d;
            var pixelFormat = PixelFormats.Gray8; // grayscale bitmap
            var bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8; // == 1 in this example

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
