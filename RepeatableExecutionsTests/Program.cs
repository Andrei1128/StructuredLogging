using Logging.ServiceExtensions;
using RepeatableExecutionsTests;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterLogger()
    .WriteTo.File(filePath: "..\\Logging\\logs")
    .WriteTo.CustomWriter<Writer>();

builder.Services.AddLoggedScoped<ITestService, TestService>();
builder.Services.AddLoggedScoped<ITestRepository, TestRepository>();
builder.Services.AddLoggedScoped<ITestRepository2, TestRepository2>();
builder.Services.AddLoggedScoped<ITestAboveRepository, TestAboveRepository>();

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