using Microsoft.OpenApi.Models;
using MassTransit;
using Contracts;
using Recalculator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Recalculator API", Version = "v1" });
});
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ResponseUsdEuroEventConsumer>();
    x.UsingRabbitMq((cntxt, cfg) =>
    {
        cfg.Host("localhost", "/", c =>
        {
            c.Username("guest");
            c.Password("guest");
        });

        cfg.ReceiveEndpoint(e => { e.Consumer<ResponseUsdEuroEventConsumer>(cntxt); });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Recalculator API v1"); });
}

var url = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://0.0.0.0:5001";
app.Urls.Clear();
app.Urls.Add(url);

app.MapGet("/max-growth-rabbit", async (string startDate, string endDate, IPublishEndpoint publishEndpoint) =>
{
    await publishEndpoint.Publish(new UsdEuroEvent { StartDate = startDate, EndDate = endDate });

    //TODO: code
    return Results.Ok(new { startDate, endDate });
});

app.MapGet("/max-growth-rest",
    (string start, string end) =>
    {
        //TODO: code
        return Results.Ok(new { start, end });
    });


app.Run();