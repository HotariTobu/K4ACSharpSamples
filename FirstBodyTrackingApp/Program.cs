// using Microsoft.Azure.Kinect.BodyTracking;
// using Microsoft.Azure.Kinect.Sensor;
using K4AdotNet.Sensor;
using K4AdotNet.BodyTracking;

// var deviceCount = Device.GetInstalledCount();
var deviceCount = Device.InstalledCount;
Console.WriteLine($"Device Count: {deviceCount}");

if (deviceCount == 0)
{
    throw new Exception("Not found: Kinect Device");
}

using (var device = Device.Open())
{
    // Console.WriteLine($"Serial Number: {device.SerialNum}");
    Console.WriteLine($"Serial Number: {device.SerialNumber}");

    var deviceConfig = new DeviceConfiguration
    {
        // CameraFPS = FPS.FPS30,
        CameraFps = FrameRate.Thirty,
        // DepthMode = DepthMode.NFOV_Unbinned,
        DepthMode = DepthMode.NarrowViewUnbinned,
        ColorResolution = ColorResolution.Off
    };
    device.StartCameras(deviceConfig);

    // var calibration = device.GetCalibration(deviceConfig.DepthMode, deviceConfig.ColorResolution);
    device.GetCalibration(deviceConfig.DepthMode, deviceConfig.ColorResolution, out var calibration);

    var trackerConfig = TrackerConfiguration.Default;
    // using (var tracker = Tracker.Create(calibration, trackerConfig))
    using (var tracker = new Tracker(calibration, trackerConfig))
    {
        using (var capture = device.GetCapture())
        {
            tracker.EnqueueCapture(capture);
        }

        using (var bodyFrame = tracker.PopResult())
        {
            Console.WriteLine($"Timestamp: {bodyFrame.DeviceTimestamp}");
            // Console.WriteLine($"Body Count: {bodyFrame.NumberOfBodies}");
            Console.WriteLine($"Body Count: {bodyFrame.BodyCount}");
        }

        tracker.Shutdown();
    }

    device.StopCameras();
}
