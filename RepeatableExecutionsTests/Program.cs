using Logging.ServiceExtensions;
using RepeatableExecutionsTests;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterLogger()
    .WriteTo.File(filePath: "..\\Logging\\logs")
    .WriteTo.AddSinksMiddleware<SinksMiddleware>()
    .WriteTo.CustomWriter<Writer>();

builder.Services.AddLoggedScoped<ITestService, TestService>();
builder.Services.AddLoggedScoped<ITestRepository, TestRepository>();
builder.Services.AddLoggedScoped<ITestRepository2, TestRepository2>();
builder.Services.AddLoggedScoped<ITestAboveRepository, TestAboveRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.CreateSinks();

app.Run();