namespace StandSystem.Core;

public interface ITcl
{
    void Start();
    void Stop();
    string GetDataFromDeviceHex();
    void SetDataToDeviceHex(string data);
}
