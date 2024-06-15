using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CaptureViewer
{
    internal class DataSource
    {
        private WriteableBitmap? _captureImage;

        public WriteableBitmap? CaptureImage
        {
            get => _captureImage;
            set => _captureImage = value;
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("CaptureImage", typeof(WriteableBitmap), typeof(DataSource), new PropertyMetadata(null));
    }
}
