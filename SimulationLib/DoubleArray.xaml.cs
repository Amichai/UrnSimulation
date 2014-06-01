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

namespace SimulationLib {
    /// <summary>
    /// Interaction logic for DoubleArray.xaml
    /// </summary>
    public partial class DoubleArray : UserControl, INotifyPropertyChanged {
        public DoubleArray() {
            InitializeComponent();
        }

        public void Clear() {
            this.init();
        }

        public double XMin {
            get { return (double)GetValue(XMinProperty); }
            set { SetValue(XMinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XMinProperty =
            DependencyProperty.Register("XMin", typeof(double), typeof(DoubleArray), new PropertyMetadata(0.0));

        public double XMax {
            get { return (double)GetValue(XMaxProperty); }
            set { SetValue(XMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XMaxProperty =
            DependencyProperty.Register("XMax", typeof(double), typeof(DoubleArray), new PropertyMetadata(0.0));

        public double YMin {
            get { return (double)GetValue(YMinProperty); }
            set { SetValue(YMinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YMinProperty =
            DependencyProperty.Register("YMin", typeof(double), typeof(DoubleArray), new PropertyMetadata(0.0));

        public double YMax {
            get { return (double)GetValue(YMaxProperty); }
            set { SetValue(YMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YMaxProperty =
            DependencyProperty.Register("YMax", typeof(double), typeof(DoubleArray), new PropertyMetadata(0.0));

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

        public int ArrayWidth {
            get { return (int)GetValue(ArrayWidthProperty); }
            set { SetValue(ArrayWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArrayWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrayWidthProperty =
            DependencyProperty.Register("ArrayWidth", typeof(int), typeof(DoubleArray), new PropertyMetadata(0));

        public int ArrayHeight {
            get { return (int)GetValue(ArrayHeightProperty); }
            set { SetValue(ArrayHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArrayHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrayHeightProperty =
            DependencyProperty.Register("ArrayHeight", typeof(int), typeof(DoubleArray), new PropertyMetadata(0));

        public byte DefaultPixelValue {
            get { return (byte)GetValue(DefaultPixelValueProperty); }
            set { SetValue(DefaultPixelValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultPixelValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultPixelValueProperty =
            DependencyProperty.Register("DefaultPixelValue", typeof(byte), typeof(DoubleArray), new PropertyMetadata((byte)0));

        private ImageSource _ImageSource;
        public ImageSource ImageSource {
            get { return _ImageSource; }
            set {
                _ImageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        public void Draw() {
            this.ImageSource = toImageSource();
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

        public double XMultiplier { 
            get {
                return this.ArrayWidth / (double)this.XRange;
            }
        }

        public double YMultiplier {
            get {
                return this.ArrayHeight / (double)this.YRange;
            }
        }

        private byte[][] frame;

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

        public double XOffset {
            get {
                return this.XRange / 2.0;
            }
        }

        public double YOffset {
            get {
                return this.YRange / 2.0;
            }
        }
        
        private byte[] toByteArray() {
            var count = this.ArrayWidth * this.ArrayHeight;
            byte[] output = new byte[count];
            //for (int i = 0; i < this.ArrayWidth; i++) {
            //    Buffer.BlockCopy(this.frame[i], 0, output, i * this.ArrayHeight, this.ArrayHeight);
            //}

            int counter = 0;
            for (int j = 0; j < this.ArrayWidth; j++) {
            //for (int j = this.ArrayWidth - 1; j >=0; j--) {

                //for (int i = 0; i < this.ArrayHeight; i++) {
                for (int i = this.ArrayHeight - 1; i >= 0; i--) {
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

            //var stride = bytesPerPixel * this.ArrayHeight; // == width in this example
            //var bitmap = BitmapSource.Create(this.ArrayHeight, this.ArrayWidth, dpiX, dpiY,
            //                                 pixelFormat, null, buffer, stride);

            var stride = bytesPerPixel * this.ArrayWidth; // == width in this example
            var bitmap = BitmapSource.Create(this.ArrayWidth, this.ArrayHeight, dpiX, dpiY,
                                             pixelFormat, null, buffer, stride);


            //var stride = bytesPerPixel * this.ArrayHeight; // == width in this example
            //var bitmap = BitmapSource.Create(this.ArrayWidth, this.ArrayHeight, dpiX, dpiY,
            //                                 pixelFormat, null, buffer, stride);
            return bitmap as BitmapSource;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) {
            var eh = PropertyChanged;
            if (eh != null) {
                eh(this, new PropertyChangedEventArgs(name));
            }
        }
        
    }
}
