using System.Globalization;
using Coravel.Invocable;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Invocables;

public class PhoneSearch: IInvocable
{
    private readonly InfluxDbService _service;
    private static readonly Random _random = new();

    public PhoneSearch(InfluxDbService service)
    {
        _service = service;
    }

    private SearchModel[] exampleQuery =
    {
        new()
        {
            Query = "SaleType oneOf Rent & (Title contains parking)",
            FirstSearchParam = "Rent",
            SecondSearchParam = "parking",
            ThirdSearchParam = ""
        },
        new()
        {
            Query = "SaleType oneOf Rent & (Title contains appartement) & (RoomQuantity equals 2|RoomQuantity equals 3|RoomQuantity equals 4|RoomQuantity greaterThanOrEqual 5)",
            FirstSearchParam = "Rent",
            SecondSearchParam = "appartement",
            ThirdSearchParam = "2, 3, 4, 5"
        },
        new()
        {
            Query = "SaleType oneOf Sale & (Title contains appartement) & (RoomQuantity equals 1)",
            FirstSearchParam = "Sale",
            SecondSearchParam = "appartement",
            ThirdSearchParam = "1"
        },
        new()
        {
            Query = "SaleType oneOf Sale & (Title contains appartement|Title contains maison) & (RoomQuantity equals 3|RoomQuantity equals 4)",
            FirstSearchParam = "Sale",
            SecondSearchParam = "appartement, maison",
            ThirdSearchParam = "3, 4"
        },
        new()
        {
            Query = "SaleType oneOf Rent & (Title contains maison) & (RoomQuantity greaterThanOrEqual 5)",
            FirstSearchParam = "Rent",
            SecondSearchParam = "maison",
            ThirdSearchParam = "5"
        },
    };
    
    private List<PointData> DefinePoint(int numberOfPoint)
    {
        List<PointData> listPoint = new List<PointData>();
        for (int i = 0; i < numberOfPoint; i++)
        {
            var rand = _random.Next(0, 5);
            var point = PointData.Measurement("Search")
                .Tag("TypeTransaction", exampleQuery[rand].FirstSearchParam)
                .Tag("TypeEstate", exampleQuery[rand].SecondSearchParam)
                .Tag("NbrRoom", exampleQuery[rand].ThirdSearchParam)
                .Field("Query", exampleQuery[rand].Query)
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);
            listPoint.Add(point);
        }
        
        return listPoint;
    } 

    public Task Invoke()
    {
        _service.Write(write =>
        {
            var rand = _random.Next(0, 4);
            var points = DefinePoint(1000);
            write.WritePoints(points, "Search", "DEV3I");
            Console.WriteLine("Search New Value");
        });
        return Task.CompletedTask;
    }
}