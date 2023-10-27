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
            var input = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(log.Entry.Input[0], settings), settings).Values.ToArray();
            foreach (var item in input)
            {
                Console.WriteLine($"Type: {item.GetType()}, Value: {item}");
            }
            var result = methodInfo.Invoke(controller, new object[] { input[0], Convert.ToInt32(input[1]), input[2] as TestObject });
        }
        else throw new InvalidOperationException($"Method '{methodToCall}' does not exist in the controller.");
    }
    public string serializedLog = @"{
  ""$type"": ""Logging.Objects.Log, Logging"",
  ""Entry"": {
    ""$type"": ""Logging.Objects.LogEntry, Logging"",
    ""Time"": ""2023-10-27T18:20:59.6747091+03:00"",
    ""Class"": ""RepeatableExecutionsTests.TestController"",
    ""Method"": ""GetWeatherEndpoint"",
    ""Input"": {
      ""$values"": [
        {
           ""data"": ""asdasd"",
          ""number"": 233,
          ""obj"": {
            ""obj2"": {
              ""name2"": ""naspa"",
              ""lastName2"": ""Stiu"",
              ""age2"": 56
            },
            ""name"": ""asd"",
            ""lastName"": ""asd2"",
            ""age"": 2
          }
        }
      ]
    }
  },
  ""Exit"": {
    ""$type"": ""Logging.Objects.LogExit, Logging"",
    ""Time"": ""2023-10-27T18:20:59.6784617+03:00"",
    ""Output"": ""asdasd 233 asd asd2 2 obj2: naspa Stiu 56""
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
          ""Time"": ""2023-10-27T18:20:59.675849+03:00"",
          ""Class"": ""RepeatableExecutionsTests.TestService"",
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
                  ""name2"": ""naspa"",
                  ""lastName2"": ""Stiu"",
                  ""age2"": 56
                },
                ""name"": ""asd"",
                ""lastName"": ""asd2"",
                ""age"": 2
              }
            ]
          }
        },
        ""Exit"": {
          ""$type"": ""Logging.Objects.LogExit, Logging"",
          ""Time"": ""2023-10-27T18:20:59.6778465+03:00"",
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
                ""Time"": ""2023-10-27T18:20:59.6768384+03:00"",
                ""Class"": ""RepeatableExecutionsTests.TestRepository"",
                ""Method"": ""Test"",
                ""Input"": {
                  ""$type"": ""System.Object[], System.Private.CoreLib"",
                  ""$values"": [
                    ""asdasd_Repository"",
                    {
                      ""$type"": ""RepeatableExecutionsTests.TestObject, RepeatableExecutionsTests"",
                      ""obj2"": {
                        ""$type"": ""RepeatableExecutionsTests.TestObject2, RepeatableExecutionsTests"",
                        ""name2"": ""naspa"",
                        ""lastName2"": ""Stiu"",
                        ""age2"": 56
                      },
                      ""name"": ""asd"",
                      ""lastName"": ""asd2"",
                      ""age"": 2
                    }
                  ]
                }
              },
              ""Exit"": {
                ""$type"": ""Logging.Objects.LogExit, Logging"",
                ""Time"": ""2023-10-27T18:20:59.6775044+03:00"",
                ""Output"": {
                  ""$type"": ""System.ValueTuple`2[[System.String, System.Private.CoreLib],[RepeatableExecutionsTests.TestObject, RepeatableExecutionsTests]], System.Private.CoreLib"",
                  ""Item1"": ""AboveTest"",
                  ""Item2"": {
                    ""$type"": ""RepeatableExecutionsTests.TestObject, RepeatableExecutionsTests"",
                    ""obj2"": {
                      ""$type"": ""RepeatableExecutionsTests.TestObject2, RepeatableExecutionsTests"",
                      ""name2"": ""naspa"",
                      ""lastName2"": ""Stiu"",
                      ""age2"": 56
                    },
                    ""name"": ""asd"",
                    ""lastName"": ""asd2"",
                    ""age"": 2
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
                      ""Time"": ""2023-10-27T18:20:59.6772214+03:00"",
                      ""Class"": ""RepeatableExecutionsTests.TestAboveRepository"",
                      ""Method"": ""Test"",
                      ""Input"": {
                        ""$type"": ""System.Object[], System.Private.CoreLib"",
                        ""$values"": [
                          ""asdasd_Repository_AboveRepository"",
                          1
                        ]
                      }
                    },
                    ""Exit"": {
                      ""$type"": ""Logging.Objects.LogExit, Logging"",
                      ""Time"": ""2023-10-27T18:20:59.6773839+03:00"",
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
                ""Time"": ""2023-10-27T18:20:59.6776711+03:00"",
                ""Class"": ""RepeatableExecutionsTests.TestRepository2"",
                ""Method"": ""Test"",
                ""Input"": {
                  ""$type"": ""System.Object[], System.Private.CoreLib"",
                  ""$values"": [
                    ""asdasd_Repository2"",
                    ""Test_Repo2""
                  ]
                }
              },
              ""Exit"": {
                ""$type"": ""Logging.Objects.LogExit, Logging"",
                ""Time"": ""2023-10-27T18:20:59.677841+03:00"",
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