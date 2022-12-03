using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using VirtualDars.CachingDemo.Infra;
using VirtualDars.CachingDemo.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CountryDbContext>(optionsAction =>
                 optionsAction.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

// In-Memory Cache hizmatini qo'shish:
builder.Services.AddMemoryCache();

// Redis-distirbuted cache hizmatini qo'shish:
builder.Services.AddStackExchangeRedisCache(setupAction =>
{
    setupAction.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
});

builder.Services.Configure<ConfigurationOptions>(builder.Configuration.GetSection("RedisCacheOptions"));
builder.Services.AddScoped<CountryRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddControllers();

// OutputCache hizmatini qo'shamiz:
builder.Services.AddOutputCache();

// ResponseCaching hizmatini qo'shamiz:
builder.Services.AddResponseCaching();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseResponseCaching(); // response caching middleware'ni qo'shamiz.

app.UseOutputCache(); // output cache middleware'ni avtorizatiyadan keyin qo'shamiz!

app.MapControllers();

app.Run();
