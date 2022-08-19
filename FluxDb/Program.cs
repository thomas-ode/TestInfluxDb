using Coravel;
using WebApplication1.Invocables;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<InfluxDbService>();
builder.Services.AddTransient<ScanQr>();
builder.Services.AddTransient<PhoneSearch>();
builder.Services.AddTransient<ReadScanData>();
builder.Services.AddTransient<ReadSearchData>();
builder.Services.AddTransient<ReadMediaData>();
builder.Services.AddTransient<DisplayMedia>();
builder.Services.AddScheduler();


var app = builder.Build();
((IApplicationBuilder) app).ApplicationServices.UseScheduler(scheduler =>
{
    scheduler.Schedule<PhoneSearch>().EveryTenSeconds();
    scheduler.Schedule<ScanQr>().EveryTenSeconds();
    scheduler.Schedule<DisplayMedia>().EveryTenSeconds();
    /*scheduler.Schedule<ReadScanData>().EveryTenSeconds();
    scheduler.Schedule<ReadMediaData>().EveryFiveSeconds();
    scheduler.Schedule<ReadSearchData>().EveryFiveSeconds();*/
});

app.MapGet("/", () => "");

app.Run();