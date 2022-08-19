using System.Collections;
using Coravel.Invocable;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Invocables;

public class ReadScanData: IInvocable
{
    private readonly InfluxDbService _service;

    public ReadScanData(InfluxDbService service)
    {
        _service = service;
    }

    public async Task Invoke()
    { 
        var scanData =  await _service.QueryAsync(async query =>
        {
            var flux = "from(bucket:\"ScanTest\") |> range(start: 0)";
            var tables = await query.QueryAsync(flux, "DEV3I");
            return tables.SelectMany(table =>
                table.Records.Select(record =>
                    new QrCode()
                    {
                        Duration = int.Parse(record.GetValue().ToString()!),
                        Time = record.GetTime().ToString()
                    }));
        });
        foreach (var data in scanData)
        {
            Console.WriteLine($"Le scan fni à {data.Time} a duré {data.Duration}sec");
        }
        var numberOfScan = scanData.Count();
        var average =  Math.Round(scanData.Average(qr => qr.Duration), 2);
        Console.WriteLine($"Il y a eu {numberOfScan} scan, pour une moyenne de {average}sec par scan");
    }
}