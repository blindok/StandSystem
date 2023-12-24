using System.Runtime.CompilerServices;

namespace StandSystem.Core;

public class Device
{
    private readonly ITcl _tcl;
    private Thread _poll;
    private object _locker;

    public string Name { get; private set; }
    public string DataFromDevice { get; private set; }
    public string DataToDevice { private set; get; }

    public Device(string name, ITcl tcl)
    {
        _tcl = tcl;
        Name = name;
    }

    public void Run()
    {
        _locker = new();
        _tcl.Start();

        _poll.Start();
    }

    public void Stop()
    {
        _locker = null;
        _poll.Abort();
    }

    public static void Poll(Device device, object locker)
    {

    }
}
