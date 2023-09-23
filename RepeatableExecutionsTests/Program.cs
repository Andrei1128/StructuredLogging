using Logging.Configurations;
using Logging.ServiceExtensions;
using RepeatableExecutionsTests;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

new LoggerConfiguration()
    .SupressExceptions();

builder.Services.InitializeLogging();

builder.Services.AddLogging<ITestService, TestService>(ServiceLifetime.Scoped);
builder.Services.AddLogging<ITestRepository, TestRepository>(ServiceLifetime.Scoped);
builder.Services.AddLogging<ITestRepository2, TestRepository2>(ServiceLifetime.Scoped);
builder.Services.AddLogging<ITestAboveRepository, TestAboveRepository>(ServiceLifetime.Scoped);

//builder.Services.AddScoped<ITestService, TestService>();
//builder.Services.AddScoped<ITestRepository, TestRepository>();
//builder.Services.AddScoped<ITestRepository2, TestRepository2>();
//builder.Services.AddScoped<ITestAboveRepository, TestAboveRepository>();

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