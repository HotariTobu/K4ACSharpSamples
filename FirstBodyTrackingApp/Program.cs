using Microsoft.Azure.Kinect.BodyTracking;
using Microsoft.Azure.Kinect.Sensor;

var deviceCount = Device.GetInstalledCount();
Console.WriteLine($"Device Count: {deviceCount}");

if (deviceCount == 0)
{
    throw new Exception("Not found: Kinect Device");
}

using (var device = Device.Open())
{
    Console.WriteLine($"Serial Number: {device.SerialNum}");

    var deviceConfig = new DeviceConfiguration
    {
        CameraFPS = FPS.FPS30,
        DepthMode = DepthMode.NFOV_Unbinned,
        ColorResolution = ColorResolution.Off
    };
    device.StartCameras(deviceConfig);

    var calibration = device.GetCalibration(deviceConfig.DepthMode, deviceConfig.ColorResolution);

    var tackerConfig = TrackerConfiguration.Default;
    using (var tracker = Tracker.Create(calibration, tackerConfig))
    {
        using (var capture = device.GetCapture())
        {
            tracker.EnqueueCapture(capture);
        }

        using (var bodyFrame = tracker.PopResult())
        {
            Console.WriteLine($"Timestamp: {bodyFrame.DeviceTimestamp}");
            Console.WriteLine($"Body Count: {bodyFrame.NumberOfBodies}");
        }

        tracker.Shutdown();
    }

    device.StopCameras();
}
