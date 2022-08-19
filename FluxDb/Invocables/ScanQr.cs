using System.Globalization;
using Coravel.Invocable;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using WebApplication1.Services;

namespace WebApplication1.Invocables;

public class ScanQr: IInvocable
{
    private readonly InfluxDbService _service;
    private static readonly Random _random = new Random();

    public ScanQr(InfluxDbService service)
    {
        _service = service;
    }
    
    private List<PointData> DefinePoint(int numberOfPoint)
    {
        List<PointData> listPoint = new List<PointData>();
        for (int i = 0; i < numberOfPoint; i++)
        {
            var duration = _random.Next(1, 59);
            var disconnected = DateTime.UtcNow;
            var point = PointData.Measurement("connectedClient")
                .Tag("QRCode", "Connexion")
                .Field("ConnectionDuration", duration)
                .Timestamp(disconnected, WritePrecision.Ns);
            listPoint.Add(point);
        }
        
        return listPoint;
    } 

    public Task Invoke()
    {
        _service.Write(write =>
        {
            var points = DefinePoint(1000);
            write.WritePoints(points, "ScanTest", "DEV3I");
            Console.WriteLine("Scan new value");
        });
        return Task.CompletedTask;
    }
}