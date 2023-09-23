using Logging.Configurations;
using Logging.ServiceExtensions;
using RepeatableExecutionsTests;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

new Logger(builder.Services)
    .SupressExceptions();

builder.Services.AddLogging<ITestService, TestService>(ServiceLifetime.Scoped);
builder.Services.AddLogging<ITestRepository, TestRepository>(ServiceLifetime.Scoped);
builder.Services.AddLogging<ITestRepository2, TestRepository2>(ServiceLifetime.Scoped);
builder.Services.AddLogging<ITestAboveRepository, TestAboveRepository>(ServiceLifetime.Scoped);

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