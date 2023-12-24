using Microsoft.AspNetCore.Mvc;
using Renci.SshNet;
using StandSystem.Models;
using StandSystem.Models.ViewModels;
using Renci.SshNet;

namespace StandSystem.Controllers;

public class StandController : Controller
{
    private readonly IDeviceManager _deviceManager;

    public StandController(IDeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
    }

    [HttpGet]
    public IActionResult Stands()
    {
        var model = new List<StandVM>
        {
            new() { Name = "Микроконтроллеры", Url = "microcontrollers" },
            new() { Name = "Устройства", Url = "devices" }
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Show(string url)
    {
        return View(url);
    }

    [HttpGet]
    [Route("/api/devices")]
    public IActionResult Devices()
    {
        return Ok(_deviceManager.Devices.Select(d => new DeviceVM { Name = d.Name }));
    }

    /// <summary>
    /// Метод для общения сервера с ПЛИС.
    /// ПЛИС обращается к серверу с информацией доступен или нет. А также с данными.
    /// В ответе передаются данные, информация о том, нужно ли перепрограммирование, путь до файла.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("/api/standExchange")]
    public IActionResult StandExchange([FromBody] StandInfoModel model)
    {
        Device device = _deviceManager.GetDevice(model.Name);
        device.Enabled = model.Enabled;
        device.DataFromDevice = model.Data;
        return Ok(new ClientInfoModel { Data = device.DataToDevice, IsProgrammNeed = false, Path = "" });
    }

    /// <summary>
    /// Метод для связи клиента (пользователя) с сервером на хостинге.
    /// Возвращает данные с выходов ПЛИС с заданным именем.
    /// </summary>
    /// <param name="deviceName">Имя ПЛИС</param>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/poll")]
    public IActionResult GetBits(string deviceName)
    {
        Device device = _deviceManager.GetDevice(deviceName);
        return Ok(new { Data = device.DataFromDevice });
    }

    /// <summary>
    /// Метод для связи клиента (пользователя) с сервером на хостинге
    /// Получает данные для устаноки их потом на входы ПЛИС
    /// </summary>
    /// <param name="model">данные для стенда</param>
    [HttpPost]
    [Route("/api/poll")]
    public void SendBits(DataToStand model)
    {
        Device device = _deviceManager.GetDevice(model.Name);
        device.DataToDevice = model.Data;
    }
}
