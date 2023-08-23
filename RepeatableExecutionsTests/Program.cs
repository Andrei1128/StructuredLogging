using RepeatableExecutionsTests.Logging;
using RepeatableExecutionsTests.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Decorate<IWeatherForecastService, WeatherForecastService>(ServiceLifetime.Scoped);
builder.Services.Decorate<IWeatherForecastService2, WeatherForecastService2>(ServiceLifetime.Scoped);

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