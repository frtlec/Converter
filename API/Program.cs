using API.Databases.Mongo;
using API.Databases.Mongo.DataAccess;
using API.Services;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<TahtakaleIntegration>();
builder.Services.AddTransient<TahtakaleService>();
builder.Services.AddTransient<FollowedProductDataAccess>();
builder.Services.AddTransient<FollowedProductService>();
builder.Services.AddSingleton<IMongoDBSettings>(sp =>
  sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);
builder.Services.Configure<MongoDBSettings>(
  builder.Configuration.GetSection(nameof(MongoDBSettings)));
builder.Services.AddCors(options =>
{
    options.AddPolicy(
              name: "AllowOrigin",
              builder =>
              {
                  builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
              });
});
var app = builder.Build();

Task.Run(async () =>
{
    while (true)
    {
        var serviceScope = app.Services.CreateScope();
        var x = serviceScope.ServiceProvider.GetRequiredService<TahtakaleIntegration>();
        await x.Sync();
        Console.WriteLine($"Sekronizasyon çalýþtý {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        await Task.Delay(1000*60*5);
    }
   
});

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();
