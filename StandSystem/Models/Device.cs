namespace StandSystem.Models;

public class Device
{
    public bool   Enabled { get; set; }
    public string Name { get; private set; }
    public string DataFromDevice { get; set; }
    public string DataToDevice { get; set; }

    public Device(string name, bool enabled)
    {
        Name = name;
        Enabled = enabled;
        DataFromDevice = "";
        DataToDevice = "";
    }
}

public class DeviceManager : IDeviceManager
{
    private List<Device> devices = new List<Device>();
    public IEnumerable<Device> Devices => devices;
    public Device GetDevice(string name)
    {
        var device = devices.FirstOrDefault(d => d.Name == name);
        if (device == null)
        {
            device = new Device(name, false);
            devices.Add(device);
        }
        return device;
    }
}

public interface IDeviceManager
{
    Device GetDevice(string name);
    IEnumerable<Device> Devices { get; }
}