using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThunderHerd.Core.Options;
using ThunderHerd.DataAccess;
using ThunderHerd.DataAccess.Interfaces;
using ThunderHerd.Domain.Handlers;
using ThunderHerd.Domain.HttpClients;
using ThunderHerd.Domain.Interfaces;
using ThunderHerd.Domain.Services;
using Hangfire;
using Hangfire.MemoryStorage;
using ThunderHerd.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add database
builder.Services.AddDbContext<ThunderHerdContext>(options =>
{
    options.UseInMemoryDatabase("RegressionDb");
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
    options.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
}, ServiceLifetime.Scoped);

// Add Hangfire background server (use InMemory while testing, use SQL/Redis for persistence)
builder.Services
    .AddHangfire(config =>
    {
        config.UseMemoryStorage();
    })
    .AddHangfireServer();

// Add config options
builder.Services
    .Configure<RunServiceOptions>(builder.Configuration.GetSection(nameof(RunServiceOptions)));

// Add services to the container.
// Important that these are added before HttpClient
builder.Services
    .AddTransient<LogRequestHandler>()
    .AddTransient<IHerdClient, HerdClient>()
    .AddScoped<IRunRepository, RunRepository>()
    .AddScoped<IRunService, RunService>()
    .AddScoped<IScheduleService, ScheduleService>();

// Add typed HttpClient
builder.Services
    .AddHttpClient<IHerdClient, HerdClient>(httpClient =>
    {
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        //httpClient.Timeout = TimeSpan.FromMinutes(120);
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            AutomaticDecompression = System.Net.DecompressionMethods.All,
            MaxConnectionsPerServer = 512,
            UseProxy = false,
        };
    })
    .AddHttpMessageHandler<LogRequestHandler>();
//.RemoveAllLoggers(); // Might not be a good idea

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        var enumConverter = new JsonStringEnumConverter();
        options.JsonSerializerOptions.Converters.Add(enumConverter);
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.AllowTrailingCommas = true;
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
