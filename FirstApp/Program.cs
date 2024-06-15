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
        ColorFormat = ImageFormat.ColorBGRA32,
        ColorResolution = ColorResolution.R1080p
    };

    device.StartCameras(deviceConfig);

    Console.WriteLine("Camera started and stopping...");

    device.StopCameras();
}
