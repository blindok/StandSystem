namespace StandSystem.Models;

public class StandInfoModel
{
    public string Data { get; set; }
    public bool Enabled { get; set; }
    public string Name { get; set; }
}

public class ClientInfoModel
{
    public string Data { get; set; }
    public bool IsProgrammNeed { get; set; }
    public string Path { get; set; }
}

public class DataToStand
{
    public string Data { get; set; }
    public string Name { get; set; }
}