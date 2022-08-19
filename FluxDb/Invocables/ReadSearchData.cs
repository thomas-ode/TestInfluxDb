using System.Collections;
using Coravel.Invocable;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Invocables;

public class ReadSearchData: IInvocable
{
    private readonly InfluxDbService _service;

    public ReadSearchData(InfluxDbService service)
    {
        _service = service;
    }

    public async Task Invoke()
    { 
        var searchData =  await _service.QueryAsync(async query =>
        {
            const string flux = "from(bucket:\"Search\") |> range(start: -6h) |> filter(fn: (r) => r.TypeTransaction == \"Rent\")";
            var tables = await query.QueryAsync(flux, "DEV3I");
            return tables.SelectMany(table =>
                table.Records.Select(record =>
                    new SearchResult()
                    {
                        QueryRes = record.GetValue().ToString(),
                        Time = Convert.ToDateTime(record.GetTime().ToString()) 
                    }));
        });
        var numberOfMedia = searchData.Count();
        Console.WriteLine($"Il y a eu {numberOfMedia} recherches avec ces paramètres ('Rent').");
    }
}