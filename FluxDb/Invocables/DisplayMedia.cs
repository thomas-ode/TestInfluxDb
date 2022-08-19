using System.Globalization;
using Coravel.Invocable;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Invocables;

public class DisplayMedia: IInvocable
{
    private readonly InfluxDbService _service;
    private static readonly Random _random = new();

    public DisplayMedia(InfluxDbService service)
    {
        _service = service;
    }

    private Media[] _listMedia = {
        new()
        {
            Id = "10",
            Duration = _random.Next(1, 59),
            Time = DateTime.UtcNow
        },
        new()
        {
            Id = "500",
            Duration = _random.Next(1, 59),
            Time = DateTime.UtcNow
        }
    };

    private List<PointData> DefinePoint(int numberOfPoint)
    {
        List<PointData> listPoint = new List<PointData>();
        for (int i = 0; i < numberOfPoint; i++)
        {
            var rand = _random.Next(0, 2);
            var point = PointData.Measurement("DisplayMedia")
                .Tag("MediaId", _listMedia[1].Id)
                .Field("DisplayTime", _listMedia[rand].Duration)
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);
            listPoint.Add(point);
        }
        
        return listPoint;
    } 
    
    public Task Invoke()
    {
        _service.Write(write =>
        {
            var points = DefinePoint(1000);
            write.WritePoints(points, "DisplayMedia", "DEV3I");
            Console.WriteLine("Display NewValue");
        });
        return Task.CompletedTask;
    }
}