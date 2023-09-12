using DAL.Interfaces;
using DAL.Repositories;
using Logic.CommandMangers;
using Logic.ExternalSystem;
using Logic.Interfaces;
using Logic.Logger;
using Logic.QueueLogic;
using Logic.RedisLogic;
using Logic.Request;
using Logic.Statistics;
using Npgsql;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddXmlSerializerFormatters(); //I added xml

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddScoped<NpgsqlConnection>(s =>
{
    return new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient(typeof(ICustomLogger<>), typeof(CustomLogger<>));
builder.Services.AddScoped<IRedisWrapper, RedisWrapper>();
builder.Services.AddScoped<IDBConnection, DBConnection>();
builder.Services.AddScoped<ICacheHandler, RedisCacheHandler>();
builder.Services.AddScoped<IExternalServiceResponseRepository, ExternalServiceResponseRepository>();
builder.Services.AddScoped<IRequestsRepository, RequestRepository>();
builder.Services.AddScoped<IQueuesHandler, QueuesHandler>();
builder.Services.AddScoped<IStatisticsHandler, StatisticsHandler>();
builder.Services.AddScoped<IRequestHandler, RequestHandler>();
builder.Services.AddScoped<IFactory, CommandManagerFactory>();
builder.Services.AddScoped<IWebClient, WebClient>();
builder.Services.AddScoped<ExternalSystemRequestSender>();
builder.Services.AddScoped<ICommandManager, FindCommandManager>();
builder.Services.AddScoped<ICommandManager, InsertCommandManager>();
builder.Services.AddScoped<IExternalSystemRequestHelper, ExternalSystemRequestHelper>();
builder.Services.AddHostedService<ExternalSystemRequestSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
