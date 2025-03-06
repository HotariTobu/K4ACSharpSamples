using System.Buffers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

// using Microsoft.Azure.Kinect.Sensor;
using K4AdotNet.Sensor;

namespace CaptureViewer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private DataSource _dataSource;

    private Device _device;
    private DeviceConfiguration _deviceConfig;

    private DispatcherTimer _timer;

    public MainWindow()
    {
        InitializeComponent();

        if (DataContext is DataSource dataSource)
        {
            _dataSource = dataSource;
        }
        else
        {
            _dataSource = new();
        }

        _dataSource.CaptureImage = new(1920, 1080, 96, 96, PixelFormats.Bgra32, null);

        _device = Device.Open();
        _deviceConfig = new DeviceConfiguration
        {
            // CameraFPS = FPS.FPS30,
            CameraFps = FrameRate.Thirty,
            // ColorFormat = ImageFormat.ColorBGRA32,
            ColorFormat = ImageFormat.ColorBgra32,
            ColorResolution = ColorResolution.R1080p
        };

        _timer = new();
        _timer.Tick += new System.EventHandler(_timerTick);
        _timer.Interval = TimeSpan.FromMilliseconds(50);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _device.Dispose();
    }

    protected override void OnActivated(EventArgs e)
    {
        base.OnActivated(e);
        _device.StartCameras(_deviceConfig);
        _timer.Start();
    }

    protected override void OnDeactivated(EventArgs e)
    {
        base.OnDeactivated(e);
        _device.StopCameras();
        _timer.Stop();
    }

    private unsafe void _timerTick(object? sender, EventArgs e)
    {
        using var capture = _device.GetCapture();
        // using var image = capture.Color;
        using var image = capture.ColorImage;
        if (image == null) {
            return;
        }

        var rect = new Int32Rect(0, 0, image.WidthPixels, image.HeightPixels);

        // using var memoryHandle = image.Memory.Pin();
        // var buffer = new IntPtr(memoryHandle.Pointer);
        var buffer = image.Buffer;

        // _dataSource.CaptureImage?.WritePixels(rect, buffer, (int)image.Size, image.StrideBytes);
        _dataSource.CaptureImage?.WritePixels(rect, buffer, image.SizeBytes, image.StrideBytes);
        _dataSource.Timestamp = image.DeviceTimestamp.ToString();
    }
}
