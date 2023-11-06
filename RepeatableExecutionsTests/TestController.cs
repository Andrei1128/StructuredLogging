using Logging.Interceptors;
using Logging.Objects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using RepeatableExecutionsTests;
using Service;
using System.Reflection;
using ILogger = Logging.Interceptors.ILogger;

namespace Tests.Objects;
[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private ITestService _testService;
    private ILogger _logger;
    public TestController(ITestService testService, ILogger logger)
    {
        _testService = testService;
        _logger = logger;
    }
    [HttpPost]
    [ServiceFilter(typeof(StructuredLoggingAttribute))]
    public string GetWeatherEndpoint(string data, int number, [FromBody] TestObject obj)
    {
        var result = _testService.Test(data, 1, obj);
        return data + " " + number + " " + obj;
    }
    [HttpGet]
    public void Replay()
    {
        Log? log = JsonConvert.DeserializeObject<Log>(serializedLog, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        var entry = log.Entry;
        Type? classType = Type.GetType(entry.Class);
        if (classType != null)
        {
            ConstructorInfo[] constructors = classType.GetConstructors();
            List<object> dependenciesList = new();
            if (constructors.Length != 0)
            {
                ConstructorInfo constructor = constructors
                .OrderByDescending(c => c.GetParameters().Length)
                .First();
                ParameterInfo[] constructorParameters = constructor.GetParameters();

                var interactions = log.Interactions;
                foreach (var param in constructorParameters)
                {
                    Type paramType = param.ParameterType;
                    Type mockType = typeof(Mock<>).MakeGenericType(paramType);
                    var mock = Activator.CreateInstance(mockType);
                    foreach (var interaction in interactions)
                    {
                        Type thisClassType = Type.GetType(interaction.Entry.Class);
                        if (thisClassType != null && paramType.IsAssignableFrom(thisClassType))
                        {
                            object output = interaction.Exit.Output;
                            string method = interaction.Entry.Method;
                            MethodInfo thisMethodInfo = thisClassType.GetMethod(method);
                            if (thisMethodInfo != null)
                            {

                                // you are here!

                                var setup = mock.GetType().GetMethod("Setup")
                                    .MakeGenericMethod(thisMethodInfo.ReturnType)
                                    .Invoke(mock, new object[] { thisMethodInfo });

                                setup.GetType().GetMethod("Returns")
                                    .MakeGenericMethod(thisMethodInfo.ReturnType)
                                    .Invoke(setup, new[] { output });
                            }
                        }
                    }
                    var mockObject = ((Mock)mock).Object;
                    dependenciesList.Add(mockObject);
                }
            }
            object[]? dependencies = dependenciesList.Count == 0 ? null : dependenciesList.ToArray();
            var instance = Activator.CreateInstance(classType, dependencies);
            var methodInfo = classType.GetMethod(entry.Method);
            if (methodInfo != null)
            {
                object[] parameters = log.Entry.Input;
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = Convert.ChangeType(parameters[i], parameterInfos[i].ParameterType);
                }
                var result = methodInfo.Invoke(instance, parameters);
            }
            else throw new InvalidOperationException($"Method '{entry.Method}' does not exist.");
        }
        else throw new InvalidOperationException($"Class '{entry.Method}' does not exist.");
    }
    public string serializedLog = @"{
  ""$type"": ""Logging.Objects.Log, Logging"",
  ""Entry"": {
    ""$type"": ""Logging.Objects.LogEntry, Logging"",
    ""Time"": ""2023-11-06T19:59:49.6687771+02:00"",
    ""Class"": ""Tests.Objects.TestController, RepeatableExecutionsTests"",
    ""Method"": ""GetWeatherEndpoint"",
    ""Input"": {
      ""$type"": ""System.Object[], System.Private.CoreLib"",
      ""$values"": [
        ""aasd"",
        12,
        {
          ""$type"": ""RepeatableExecutionsTests.TestObject, RepeatableExecutionsTests"",
          ""obj2"": {
            ""$type"": ""RepeatableExecutionsTests.TestObject2, RepeatableExecutionsTests"",
            ""name2"": ""string"",
            ""numbers"": {
              ""$type"": ""System.Int32[], System.Private.CoreLib"",
              ""$values"": [
                0
              ]
            },
            ""age2"": 0
          },
          ""name"": ""string"",
          ""lastName"": ""string"",
          ""age"": 0
        }
      ]
    }
  },
  ""Exit"": {
    ""$type"": ""Logging.Objects.LogExit, Logging"",
    ""Time"": ""2023-11-06T19:59:49.671236+02:00"",
    ""Output"": ""aasd 12 string string 0 obj2: string 0""
  },
  ""Infos"": {
    ""$type"": ""System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib"",
    ""$values"": [
    ]
  },
  ""Interactions"": {
    ""$type"": ""System.Collections.Generic.List`1[[Logging.Objects.Log, Logging]], System.Private.CoreLib"",
    ""$values"": [
      {
        ""$type"": ""Logging.Objects.Log, Logging"",
        ""Entry"": {
          ""$type"": ""Logging.Objects.LogEntry, Logging"",
          ""Time"": ""2023-11-06T19:59:49.6699587+02:00"",
          ""Class"": ""Service.TestService, RepeatableExecutionsTests"",
          ""Method"": ""Test"",
          ""Input"": {
            ""$type"": ""System.Object[], System.Private.CoreLib"",
            ""$values"": [
              ""aasd"",
              1,
              {
                ""$type"": ""RepeatableExecutionsTests.TestObject, RepeatableExecutionsTests"",
                ""obj2"": {
                  ""$type"": ""RepeatableExecutionsTests.TestObject2, RepeatableExecutionsTests"",
                  ""name2"": ""string"",
                  ""numbers"": {
                    ""$type"": ""System.Int32[], System.Private.CoreLib"",
                    ""$values"": [
                      0
                    ]
                  },
                  ""age2"": 0
                },
                ""name"": ""string"",
                ""lastName"": ""string"",
                ""age"": 0
              }
            ]
          }
        },
        ""Exit"": {
          ""$type"": ""Logging.Objects.LogExit, Logging"",
          ""Time"": ""2023-11-06T19:59:49.6706008+02:00"",
          ""Output"": ""Test_Service""
        },
        ""Infos"": {
          ""$type"": ""System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib"",
          ""$values"": [
          ]
        },
        ""Interactions"": {
          ""$type"": ""System.Collections.Generic.List`1[[Logging.Objects.Log, Logging]], System.Private.CoreLib"",
          ""$values"": [
          ]
        }
      }
    ]
  }
}";
}