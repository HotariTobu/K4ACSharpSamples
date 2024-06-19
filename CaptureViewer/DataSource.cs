using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using SharedWPF;

namespace CaptureViewer
{
    internal class DataSource : ViewModelBase
    {
        #region == CaptureImage ==

        private WriteableBitmap? _captureImage;
        public WriteableBitmap? CaptureImage
        {
            get => _captureImage;
            set
            {
                if (_captureImage != value)
                {
                    _captureImage = value;
                    RaisePropertyChanged(nameof(CaptureImage));
                }
            }
        }

        #endregion
        #region == Timestamp ==

        private string? _timestamp;
        public string? Timestamp
        {
            get => _timestamp;
            set
            {
                if (_timestamp != value)
                {
                    _timestamp = value;
                    RaisePropertyChanged(nameof(Timestamp));
                }
            }
        }

        #endregion
    }
}
