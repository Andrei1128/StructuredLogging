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

        var dependencies = new object[]
        {
            new Mock<ITestService>().Object,
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
    ""Time"": ""2023-10-29T18:23:48.428248+02:00"",
    ""Class"": ""Tests.Objects.TestController, RepeatableExecutionsTests"",
    ""Method"": ""GetWeatherEndpoint"",
    ""Input"": {
      ""$type"": ""System.Object[], System.Private.CoreLib"",
      ""$values"": [
        ""asd"",
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
    ""Time"": ""2023-10-29T18:23:48.4321818+02:00"",
    ""Output"": ""asd 12 string string 0 obj2: string 0""
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
          ""Time"": ""2023-10-29T18:23:48.4294087+02:00"",
          ""Class"": ""Service.TestService, RepeatableExecutionsTests"",
          ""Method"": ""Test"",
          ""Input"": {
            ""$type"": ""System.Object[], System.Private.CoreLib"",
            ""$values"": [
              ""asd"",
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
          ""Time"": ""2023-10-29T18:23:48.4313145+02:00"",
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
            {
              ""$type"": ""Logging.Objects.Log, Logging"",
              ""Entry"": {
                ""$type"": ""Logging.Objects.LogEntry, Logging"",
                ""Time"": ""2023-10-29T18:23:48.4302581+02:00"",
                ""Class"": ""RepeatableExecutionsTests.TestRepository, RepeatableExecutionsTests"",
                ""Method"": ""Test"",
                ""Input"": {
                  ""$type"": ""System.Object[], System.Private.CoreLib"",
                  ""$values"": [
                    ""asd_Repository"",
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
                ""Time"": ""2023-10-29T18:23:48.4309731+02:00"",
                ""Output"": {
                  ""$type"": ""System.ValueTuple`2[[System.String, System.Private.CoreLib],[RepeatableExecutionsTests.TestObject, RepeatableExecutionsTests]], System.Private.CoreLib"",
                  ""Item1"": ""AboveTest"",
                  ""Item2"": {
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
                }
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
                      ""Time"": ""2023-10-29T18:23:48.4306122+02:00"",
                      ""Class"": ""RepeatableExecutionsTests.TestAboveRepository, RepeatableExecutionsTests"",
                      ""Method"": ""Test"",
                      ""Input"": {
                        ""$type"": ""System.Object[], System.Private.CoreLib"",
                        ""$values"": [
                          ""asd_Repository_AboveRepository"",
                          1
                        ]
                      }
                    },
                    ""Exit"": {
                      ""$type"": ""Logging.Objects.LogExit, Logging"",
                      ""Time"": ""2023-10-29T18:23:48.430855+02:00"",
                      ""Output"": ""AboveTest""
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
            },
            {
              ""$type"": ""Logging.Objects.Log, Logging"",
              ""Entry"": {
                ""$type"": ""Logging.Objects.LogEntry, Logging"",
                ""Time"": ""2023-10-29T18:23:48.4311301+02:00"",
                ""Class"": ""RepeatableExecutionsTests.TestRepository2, RepeatableExecutionsTests"",
                ""Method"": ""Test"",
                ""Input"": {
                  ""$type"": ""System.Object[], System.Private.CoreLib"",
                  ""$values"": [
                    ""asd_Repository2"",
                    ""Test_Repo2""
                  ]
                }
              },
              ""Exit"": {
                ""$type"": ""Logging.Objects.LogExit, Logging"",
                ""Time"": ""2023-10-29T18:23:48.4313088+02:00"",
                ""Output"": null
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
      }
    ]
  }
}";
}