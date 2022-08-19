using System.Collections;
using Coravel.Invocable;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Invocables;

public class ReadMediaData: IInvocable
{
    private readonly InfluxDbService _service;

    public ReadMediaData(InfluxDbService service)
    {
        _service = service;
    }

    public async Task Invoke()
    { 
        var mediaData =  await _service.QueryAsync(async query =>
        {
            const string flux = "from(bucket:\"DisplayMedia\") |> range(start: -1h) |> filter(fn: (r) => r.MediaId == \"500\")";
            var tables = await query.QueryAsync(flux, "DEV3I");
            return tables.SelectMany(table =>
                table.Records.Select(record =>
                    new Media()
                    {
                        Duration = int.Parse(record.GetValue().ToString()!),
                        Time = Convert.ToDateTime(record.GetTime().ToString()) 
                    }));
        });
        var test = 0;
        foreach (var media in mediaData)
        {
            test = test + media.Duration;
        }
        var numberOfMedia = mediaData.Count();
        Console.WriteLine($"Ce média a été affiché {numberOfMedia} fois, pour un total de {test}sec.");
    }
}