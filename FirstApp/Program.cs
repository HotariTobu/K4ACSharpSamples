// using Microsoft.Azure.Kinect.Sensor;
using K4AdotNet.Sensor;

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
        // ColorFormat = ImageFormat.ColorBGRA32,
        DepthMode = DepthMode.PassiveIR,
        ColorResolution = ColorResolution.Off
    };

    device.StartCameras(deviceConfig);

    Console.WriteLine("Camera started and stopping...");

    device.StopCameras();
}
