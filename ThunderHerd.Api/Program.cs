using System.Text.Json.Serialization;
using ThunderHerd.Core.Options;
using ThunderHerd.Domain.Handlers;
using ThunderHerd.Domain.HttpClients;
using ThunderHerd.Domain.Interfaces;
using ThunderHerd.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add config options
builder.Services
    .Configure<RunServiceOptions>(builder.Configuration.GetSection(nameof(RunServiceOptions)));

// Add services to the container.
// Important that these are added before HttpClient
builder.Services
    .AddTransient<LogRequestHandler>()
    .AddTransient<IHerdClient, HerdClient>()
    .AddScoped<IRunService, RunService>();

// Add typed HttpClient
builder.Services
    .AddHttpClient<IHerdClient, HerdClient>(httpClient =>
    {
        //httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.Timeout = TimeSpan.FromMinutes(10);
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(10))
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            AutomaticDecompression = System.Net.DecompressionMethods.All,
            MaxConnectionsPerServer = 256,
            UseDefaultCredentials = true,
            AllowAutoRedirect = false,
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
