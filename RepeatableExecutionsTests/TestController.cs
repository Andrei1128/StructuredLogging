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
    public void Test()
    {
        Log? log = JsonConvert.DeserializeObject<Log>(serializedLog, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        //////////////////////////////////////////////////////////

        var serviceLog = log.Interactions[0];
        var serviceEntry = serviceLog.Entry;
        Type serviceType = Type.GetType(serviceEntry.Class)!;
        var service = Activator.CreateInstance(serviceType);
        var serviceMethod = serviceType.GetMethod(serviceEntry.Method);
        object[] serviceParams = log.Entry.Input;
        ParameterInfo[] serviceParamInfo = serviceMethod.GetParameters();
        for (int i = 0; i < serviceParams.Length; i++)
        {
            serviceParams[i] = Convert.ChangeType(serviceParams[i], serviceParamInfo[i].ParameterType);
        }
        var serviceResult = serviceMethod.Invoke(service, serviceParams);

        //////////////////////////////////////////////////////////
        var dependencies = new object[]
        {
            //those will come from the log
            service,
            new Mock<ILogger>().Object
        };

        var entry = log.Entry;
        Type classType = Type.GetType(entry.Class)!;
        if (classType != null)
        {
            var controller = Activator.CreateInstance(classType, dependencies);
            var methodInfo = classType.GetMethod(entry.Method);
            if (methodInfo != null)
            {
                object[] parameters = log.Entry.Input;
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = Convert.ChangeType(parameters[i], parameterInfos[i].ParameterType);
                }
                var result = methodInfo.Invoke(controller, parameters);
            }
            else throw new InvalidOperationException($"Method '{entry.Method}' does not exist.");
        }
        else throw new InvalidOperationException($"Class '{entry.Method}' does not exist.");
    }
    public string serializedLog = @"{
  ""$type"": ""Logging.Objects.Log, Logging"",
  ""Entry"": {
    ""$type"": ""Logging.Objects.LogEntry, Logging"",
    ""Time"": ""2023-10-29T19:07:14.1210023+02:00"",
    ""Class"": ""Tests.Objects.TestController, RepeatableExecutionsTests"",
    ""Method"": ""GetWeatherEndpoint"",
    ""Input"": {
      ""$type"": ""System.Object[], System.Private.CoreLib"",
      ""$values"": [
        ""asdasd"",
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
    ""Time"": ""2023-10-29T19:07:14.123511+02:00"",
    ""Output"": ""asdasd 12 string string 0 obj2: string 0""
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
          ""Time"": ""2023-10-29T19:07:14.1222241+02:00"",
          ""Class"": ""Service.TestService, RepeatableExecutionsTests"",
          ""Method"": ""Test"",
          ""Input"": {
            ""$type"": ""System.Object[], System.Private.CoreLib"",
            ""$values"": [
              ""asdasd"",
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
          ""Time"": ""2023-10-29T19:07:14.1228781+02:00"",
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