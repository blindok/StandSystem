using Newtonsoft.Json;
using StandSystem.Core;
using System.Net;
using System.Text;
using Renci.SshNet;

namespace StandSystem.Service;

public class Program
{
    //формат аргументов для запуска
    //<url сервера> 
    //<название устройства (любое)>
    //<тип подключения fake - для заглушки, station - FPGA Spartan 6>
    //пример аргументов для фейкого локального стенда:
    //https://localhost:44332/ FPGA1 fake
    //пример аргументов для реального локального стенда в релизе:
    //https://localhost:5001/ FPGA1 station
    static void Main(string[] args)
    {
        var count = args.Length;
        string? url;
        string? name;
        string? mode = null;

        if (count == 0)
        {
            Console.Write("Enter server url: ");
            url = Console.ReadLine();
        }
        else
        {
            url = args[0];
        }

        if (count == 0 || count == 1)
        {
            Console.Write("Enter board name: ");
            name = Console.ReadLine();
        }
        else
        {
            name = args[1];
        }

        if (count > 2)
        {
            mode = args[2];
        }

        while (mode != "station" && mode != "portable" && mode != "fake")
        {
            Console.Write("Choose mode (\"station\", \"portable\", \"fake\"): ");
            mode = Console.ReadLine();
        }

        Console.WriteLine($"FPGA service. FPGA: \"{name}\", Url: \"{url}\", Mode \"{mode}\".");
        Console.Write("Press Y to continue or N to enter settings again. ");
        var ch = Console.ReadLine();
        while (ch != "Y" && ch != "y")
        {
            Console.Write("Enter server url: ");
            url = Console.ReadLine();
            Console.Write("Enter board name: ");
            name = Console.ReadLine();
            mode = null;
            while (mode != "station" && mode != "portable" && mode != "fake")
            {
                Console.Write("Choose mode (\"station\", \"portable\", \"fake\"): ");
                mode = Console.ReadLine();
            }
            Console.WriteLine($"FPGA service. FPGA: \"{name}\", Url: \"{url}\", Mode \"{mode}\".");
            Console.Write("Press Y to continue or N to enter settings again: ");
            ch = Console.ReadLine();
        }

        Console.WriteLine("FPGA service started");

        try
        {
            Device device;

            if (mode == "fake")
            {
                device = new Device(name, new FakeTcl());
            }
            else if (mode == "station")
            {
                device = new Device(name, new TclXilinx());
            }
            else if (mode == "portable")
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new ArgumentException();
            }

            Poll(device, url);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    static void Poll(Device device, string url)
    {
        device.Run();
        while (true)
        {
            StandInfoModel fromStandInfo = new StandInfoModel { Name = device.Name, Data = device.DataFromDevice };
            ClientInfoModel toStandInfo = SendData(url, fromStandInfo, device.Name);
            //device.DataToDevice = toStandInfo.Data;
        }
    }

    static ClientInfoModel SendData(string url, StandInfoModel requestInfo, string name)
    {
        string privateKey = "";
        var pk = new PrivateKeyFile(privateKey);
        var keyFiles = new[] { pk };
        var authMethod = new PrivateKeyAuthenticationMethod(name, keyFiles);

        ConnectionInfo connection = new ConnectionInfo($"{url}/api/standExchange", name, authMethod);

        using (var sshClient = new SshClient(connection))
        {
            sshClient.Connect();

            sshClient.Disconnect();
        }

        WebRequest request = WebRequest.Create($"{url}/api/standExchange");
        request.Method = "POST";

        string requestData = JsonConvert.SerializeObject(requestInfo);
        byte[] byteArray = Encoding.UTF8.GetBytes(requestData);

        request.ContentType = "application/json";
        request.ContentLength = byteArray.Length;

        using (Stream dataStream = request.GetRequestStream())
        {
            dataStream.Write(byteArray, 0, byteArray.Length);
        }

        WebResponse response = request.GetResponse();
        ClientInfoModel responseInfo;

        using (Stream stream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                var responseData = reader.ReadToEnd();
                responseInfo = JsonConvert.DeserializeObject<ClientInfoModel>(responseData);
            }
        }
        response.Close();

        Console.WriteLine($"Request executed...{responseInfo.Data},{responseInfo.IsProgrammNeed},{responseInfo.Path}");

        return responseInfo;
    }
}
