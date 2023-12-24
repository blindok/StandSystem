using System.Diagnostics;

namespace StandSystem.Core;

public class TclXilinx : ITcl
{
    private Process process;
    private ProcessStartInfo startInfo;

    public TclXilinx()
    {
        string file = @"C:\xilinx\14.7\ISE_DS\ISE\bin\nt\cs_tcl.tcl";
        if (!File.Exists(file))
            throw new Exception($"File {file} not found");

        file = @"c:\xilinx\14.7\ISE_DS\ISE\bin\nt\xtclsh.exe";
        if (!File.Exists(file))
            throw new Exception($"File {file} not found");
    }

    public string GetDataFromDeviceHex()
    {
        Console.WriteLine("Reading......   ");
        process.StandardInput.WriteLine("0000000000000000 0 3");
        string arrayhex = process.StandardOutput.ReadLine();
        Console.WriteLine(arrayhex);
        return Decoder.HexToBinary(arrayhex);

    }

    public void SetDataToDeviceHex(string line)
    {
        string modebit = "1";
        while (line.Length < 64)
            line += '0';
        string arrayhex = Decoder.BinaryToHex(line);
        Console.WriteLine("Write:    " + arrayhex + " " + modebit + " " + "3");
        process.StandardInput.WriteLine(arrayhex + " " + modebit + " " + "3");
        Console.WriteLine("arrayhex = " + arrayhex + " complete ");
    }

    public void Start()
    {
        startInfo = new ProcessStartInfo
        {
            FileName = "cmd",
            WorkingDirectory = @"C:\Xilinx\14.7\ISE_DS\ISE\bin\nt",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            Arguments = "/c xtclsh.exe cs_tcl.tcl -dig"
        };
        process = new Process();
        process.StartInfo = startInfo;
        process.Start();
    }

    public void Stop()
    {
        process.Kill();
    }
}
