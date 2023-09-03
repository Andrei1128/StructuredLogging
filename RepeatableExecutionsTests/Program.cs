using RepeatableExecutionsTests.Logging;
using RepeatableExecutionsTests.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.InitializeLogging();

//builder.Services.AddLogging<IWeatherForecastService2, WeatherForecastService2>(ServiceLifetime.Scoped);
//builder.Services.AddLogging<IWeatherForecastService, WeatherForecastService>(ServiceLifetime.Scoped);

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddScoped<IWeatherForecastService2, WeatherForecastService2>();

builder.Services.AddStructuredLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();