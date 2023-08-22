using StructuredLogging.Attributes;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

ApplyLoggingProxy();

app.Run();

void ApplyLoggingProxy()
{
    var assembly = typeof(Program).Assembly;
    var types = assembly.GetTypes();

    foreach (var type in types)
    {
        var methods = type.GetMethods().Where(method => method.GetCustomAttributes(typeof(LogAttribute), false).Length > 0);

        foreach (var method in methods)
        {
            ModifyMethod(type, method);
        }
    }
}

Delegate ToDelegate(Type type, MethodInfo methodInfo)
{
    var returnType = methodInfo.ReturnType;
    var delegateType = Expression.GetDelegateType(
        methodInfo.GetParameters().Select(p => p.ParameterType).Concat(new[] { returnType }).ToArray()
    );

    var dynamicMethod = new DynamicMethod(
        "DynamicMethod_" + methodInfo.Name,
        returnType,
        methodInfo.GetParameters().Select(p => p.ParameterType).ToArray(),
        type
    );

    var il = dynamicMethod.GetILGenerator();
    il.Emit(OpCodes.Ldnull);
    for (var i = 0; i < methodInfo.GetParameters().Length; i++)
    {
        il.Emit(OpCodes.Ldarg, i);
    }
    il.Emit(OpCodes.Call, methodInfo);
    il.Emit(OpCodes.Ret);

    return dynamicMethod.CreateDelegate(delegateType);
}

void ModifyMethod(Type type, MethodInfo methodInfo)
{
    var originalMethodDelegate = ToDelegate(type, methodInfo);

    Action modifiedMethod = () =>
    {
        Console.WriteLine($"Logging Before {methodInfo.Name}");
        originalMethodDelegate.DynamicInvoke();
        Console.WriteLine($"Logging After {methodInfo.Name}");
    };
    //Problem here!
    var delegateHolderField = type.GetField(
        methodInfo.Name,
        BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
    );

    if (delegateHolderField != null)
    {
        delegateHolderField.SetValue(null, modifiedMethod);
        Console.WriteLine($"Method {methodInfo.Name} has been modified.");
    }
    else
    {
        Console.WriteLine($"Failed to modify method {methodInfo.Name}.");
    }
}