using System;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using References;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "References API", Version = "v1" });
});

builder.Services.AddMassTransit(mt => mt.AddMassTransit(x =>
{
    x.AddConsumer<UsdEuroEventConsumer>();

    // x.UsingRabbitMq((cntxt, cfg) =>
    // {
    //     cfg.Host("localhost", "/", h =>
    //     {
    //         h.Username("guest");
    //         h.Password("guest");
    //     });
    //     cfg.ReceiveEndpoint(e => { e.Consumer<UsdEuroEventConsumer>(cntxt); });
    // });
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("amqps://jawqdhwu:g76FLVGq6RdBl-HbtlANul0d7B6uWESt@cow.rmq2.cloudamqp.com/jawqdhwu"));
        cfg.ReceiveEndpoint(e => { e.Consumer<UsdEuroEventConsumer>(context); });
    });
}));

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "References API v1"); });
}

var url = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://0.0.0.0:5003";
app.Urls.Clear();
app.Urls.Add(url);


app.MapGet("/usd-euro",
    (DateTime startDate, DateTime endDate) => { return Results.Ok(new { startDate, endDate }); });

app.Run();