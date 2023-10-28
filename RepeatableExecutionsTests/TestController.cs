using Logging.Interceptors;
using Logging.Objects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using ILogger = Logging.Interceptors.ILogger;

namespace RepeatableExecutionsTests;
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
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        Log? log = JsonConvert.DeserializeObject<Log>(serializedLog, settings);

        var testServiceMock = new Mock<ITestService>();
        var loggerMock = new Mock<ILogger>();
        var controller = new TestController(testServiceMock.Object, loggerMock.Object);


        string methodToCall = log.Entry.Method;
        var methodInfo = typeof(TestController).GetMethod(methodToCall);
        if (methodInfo != null)
        {
            foreach (var item in log.Entry.Input)
            {
                Console.WriteLine($"Type: {item.GetType()}, Value: {item}");
            }
            var result = methodInfo.Invoke(controller, log.Entry.Input);
        }
        else throw new InvalidOperationException($"Method '{methodToCall}' does not exist in the controller.");
    }
    public string serializedLog = @"{
  ""$type"": ""Logging.Objects.Log, Logging"",
  ""Entry"": {
    ""$type"": ""Logging.Objects.LogEntry, Logging"",
    ""Time"": ""2023-10-28T17:52:30.3702128+03:00"",
    ""Class"": ""RepeatableExecutionsTests.TestController"",
    ""Method"": ""GetWeatherEndpoint"",
    ""Input"": {
      ""$type"": ""System.Object[], System.Private.CoreLib"",
      ""$values"": [
        ""Dataaa"",
        12,
        {
          ""$type"": ""RepeatableExecutionsTests.TestObject, RepeatableExecutionsTests"",
          ""obj2"": {
            ""$type"": ""RepeatableExecutionsTests.TestObject2, RepeatableExecutionsTests"",
            ""name2"": ""naspa"",
            ""numbers"": {
              ""$type"": ""System.Int32[], System.Private.CoreLib"",
              ""$values"": [
                1,
                2,
                3
              ]
            },
            ""age2"": 56
          },
          ""name"": ""Andrei"",
          ""lastName"": ""Ionut"",
          ""age"": 29
        }
      ]
    }
  },
  ""Exit"": {
    ""$type"": ""Logging.Objects.LogExit, Logging"",
    ""Time"": ""2023-10-28T17:52:30.3737162+03:00"",
    ""Output"": ""Dataaa 12 Andrei Ionut 29 obj2: naspa 56""
  },
  ""Infos"": {
    ""$type"": ""System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib"",
    ""$values"": []
  },
  ""Interactions"": {
    ""$type"": ""System.Collections.Generic.List`1[[Logging.Objects.Log, Logging]], System.Private.CoreLib"",
    ""$values"": [
      {
        ""$type"": ""Logging.Objects.Log, Logging"",
        ""Entry"": {
          ""$type"": ""Logging.Objects.LogEntry, Logging"",
          ""Time"": ""2023-10-28T17:52:30.3712923+03:00"",
          ""Class"": ""RepeatableExecutionsTests.TestService"",
          ""Method"": ""Test"",
          ""Input"": {
            ""$type"": ""System.Object[], System.Private.CoreLib"",
            ""$values"": [
              ""Dataaa"",
              1,
              {
                ""$type"": ""RepeatableExecutionsTests.TestObject, RepeatableExecutionsTests"",
                ""obj2"": {
                  ""$type"": ""RepeatableExecutionsTests.TestObject2, RepeatableExecutionsTests"",
                  ""name2"": ""naspa"",
                  ""numbers"": {
                    ""$type"": ""System.Int32[], System.Private.CoreLib"",
                    ""$values"": [
                      1,
                      2,
                      3
                    ]
                  },
                  ""age2"": 56
                },
                ""name"": ""Andrei"",
                ""lastName"": ""Ionut"",
                ""age"": 29
              }
            ]
          }
        },
        ""Exit"": {
          ""$type"": ""Logging.Objects.LogExit, Logging"",
          ""Time"": ""2023-10-28T17:52:30.3729622+03:00"",
          ""Output"": ""Test_Service""
        },
        ""Infos"": {
          ""$type"": ""System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib"",
          ""$values"": []
        },
        ""Interactions"": {
          ""$type"": ""System.Collections.Generic.List`1[[Logging.Objects.Log, Logging]], System.Private.CoreLib"",
          ""$values"": [
            {
              ""$type"": ""Logging.Objects.Log, Logging"",
              ""Entry"": {
                ""$type"": ""Logging.Objects.LogEntry, Logging"",
                ""Time"": ""2023-10-28T17:52:30.3720344+03:00"",
                ""Class"": ""RepeatableExecutionsTests.TestRepository"",
                ""Method"": ""Test"",
                ""Input"": {
                  ""$type"": ""System.Object[], System.Private.CoreLib"",
                  ""$values"": [
                    ""Dataaa_Repository"",
                    {
                      ""$type"": ""RepeatableExecutionsTests.TestObject, RepeatableExecutionsTests"",
                      ""obj2"": {
                        ""$type"": ""RepeatableExecutionsTests.TestObject2, RepeatableExecutionsTests"",
                        ""name2"": ""naspa"",
                        ""numbers"": {
                          ""$type"": ""System.Int32[], System.Private.CoreLib"",
                          ""$values"": [
                            1,
                            2,
                            3
                          ]
                        },
                        ""age2"": 56
                      },
                      ""name"": ""Andrei"",
                      ""lastName"": ""Ionut"",
                      ""age"": 29
                    }
                  ]
                }
              },
              ""Exit"": {
                ""$type"": ""Logging.Objects.LogExit, Logging"",
                ""Time"": ""2023-10-28T17:52:30.372645+03:00"",
                ""Output"": {
                  ""$type"": ""System.ValueTuple`2[[System.String, System.Private.CoreLib],[RepeatableExecutionsTests.TestObject, RepeatableExecutionsTests]], System.Private.CoreLib"",
                  ""Item1"": ""AboveTest"",
                  ""Item2"": {
                    ""$type"": ""RepeatableExecutionsTests.TestObject, RepeatableExecutionsTests"",
                    ""obj2"": {
                      ""$type"": ""RepeatableExecutionsTests.TestObject2, RepeatableExecutionsTests"",
                      ""name2"": ""naspa"",
                      ""numbers"": {
                        ""$type"": ""System.Int32[], System.Private.CoreLib"",
                        ""$values"": [
                          1,
                          2,
                          3
                        ]
                      },
                      ""age2"": 56
                    },
                    ""name"": ""Andrei"",
                    ""lastName"": ""Ionut"",
                    ""age"": 29
                  }
                }
              },
              ""Infos"": {
                ""$type"": ""System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib"",
                ""$values"": []
              },
              ""Interactions"": {
                ""$type"": ""System.Collections.Generic.List`1[[Logging.Objects.Log, Logging]], System.Private.CoreLib"",
                ""$values"": [
                  {
                    ""$type"": ""Logging.Objects.Log, Logging"",
                    ""Entry"": {
                      ""$type"": ""Logging.Objects.LogEntry, Logging"",
                      ""Time"": ""2023-10-28T17:52:30.3723737+03:00"",
                      ""Class"": ""RepeatableExecutionsTests.TestAboveRepository"",
                      ""Method"": ""Test"",
                      ""Input"": {
                        ""$type"": ""System.Object[], System.Private.CoreLib"",
                        ""$values"": [
                          ""Dataaa_Repository_AboveRepository"",
                          1
                        ]
                      }
                    },
                    ""Exit"": {
                      ""$type"": ""Logging.Objects.LogExit, Logging"",
                      ""Time"": ""2023-10-28T17:52:30.3725241+03:00"",
                      ""Output"": ""AboveTest""
                    },
                    ""Infos"": {
                      ""$type"": ""System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib"",
                      ""$values"": []
                    },
                    ""Interactions"": {
                      ""$type"": ""System.Collections.Generic.List`1[[Logging.Objects.Log, Logging]], System.Private.CoreLib"",
                      ""$values"": []
                    }
                  }
                ]
              }
            },
            {
              ""$type"": ""Logging.Objects.Log, Logging"",
              ""Entry"": {
                ""$type"": ""Logging.Objects.LogEntry, Logging"",
                ""Time"": ""2023-10-28T17:52:30.3727872+03:00"",
                ""Class"": ""RepeatableExecutionsTests.TestRepository2"",
                ""Method"": ""Test"",
                ""Input"": {
                  ""$type"": ""System.Object[], System.Private.CoreLib"",
                  ""$values"": [
                    ""Dataaa_Repository2"",
                    ""Test_Repo2""
                  ]
                }
              },
              ""Exit"": {
                ""$type"": ""Logging.Objects.LogExit, Logging"",
                ""Time"": ""2023-10-28T17:52:30.3729571+03:00"",
                ""Output"": null
              },
              ""Infos"": {
                ""$type"": ""System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib"",
                ""$values"": []
              },
              ""Interactions"": {
                ""$type"": ""System.Collections.Generic.List`1[[Logging.Objects.Log, Logging]], System.Private.CoreLib"",
                ""$values"": []
              }
            }
          ]
        }
      }
    ]
  }
}";
}