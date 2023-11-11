using Castle.DynamicProxy;
using Logging.Logging.Objects;
using Moq;
using Newtonsoft.Json;
using RepeatableExecutionsTests;
using System.Reflection;

namespace Logging.Replaying.Objects;

public class Replayer
{
    private readonly ProxyGenerator proxyGenerator = new();
    public void Replay()
    {
        string serializedLog = File.ReadAllText("C:\\Users\\Andrei\\Facultate\\C#\\RepeatableExecutionsTests\\Logging\\logs\\GetWeatherEndpoint-202311112046292981154");
        var jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        Log log = JsonConvert.DeserializeObject<Log>(serializedLog, jsonSettings);

        Type classType = Type.GetType(log.Entry.Class);
        object[]? dependencies = CreateMockedDependencies(classType, log.Interactions);

        foreach (var dep in dependencies)
        {
            Console.WriteLine(dep.GetType());
        }
        var classInstance = Activator.CreateInstance(classType, dependencies);
        if (classInstance != null)
        {
            var methodInfo = classType.GetMethod(log.Entry.Method);
            if (methodInfo != null)
            {
                Invoke(classInstance, methodInfo, log.Entry.Input);
            }
        }
    }
    private void Invoke(object classInstance, MethodInfo methodInfo, object[] inputs)
    {
        NormalizeInputs(methodInfo, inputs);
        methodInfo.Invoke(classInstance, inputs);
    }
    private void NormalizeInputs(MethodInfo methodInfo, object[] inputs)
    {
        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = Convert.ChangeType(inputs[i], parameterInfos[i].ParameterType);
        }
    }
    private ParameterInfo[] GetConstructorParameters(Type classType)
    {
        ConstructorInfo[] constructors = classType.GetConstructors();
        if (constructors.Any())
        {
            ConstructorInfo constructor = constructors
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            return constructor.GetParameters();
        }
        return Array.Empty<ParameterInfo>();
    }
    private object[]? CreateDummyDependencies(Type classType)
    {
        List<object> dependenciesList = new();
        ParameterInfo[] constructorParameters = GetConstructorParameters(classType);
        if (constructorParameters.Any())
        {
            foreach (var param in constructorParameters)
            {
                Type paramType = param.ParameterType;
                Type genericMockType = typeof(Mock<>).MakeGenericType(paramType);
                var mock = Activator.CreateInstance(genericMockType);
                dependenciesList.Add(((Mock)mock).Object);
            }
        }
        return dependenciesList.Any() ? dependenciesList.ToArray() : null;
    }
    private object[]? CreateMockedDependencies(Type classType, List<Log> interactions)
    {
        List<object> dependenciesList = new();
        ParameterInfo[] constructorParameters = GetConstructorParameters(classType);
        if (constructorParameters.Any())
        {
            foreach (var parameter in constructorParameters)
            {
                Type parameterType = parameter.ParameterType;

                var classNameList = from log in interactions
                                    where parameterType.IsAssignableFrom(Type.GetType(log.Entry.Class))
                                    select log.Entry.Class;

                if (classNameList.Any())
                {
                    var mocksList = from log in interactions
                                    where log.Entry.Class == classNameList.First()
                                    select new MockObject(log.Entry.Method, log.Exit.Output);

                    var mockInterceptor = new MockInterceptor(mocksList);
                    var proxiedMock = proxyGenerator.CreateInterfaceProxyWithoutTarget(parameterType, mockInterceptor);
                    dependenciesList.Add(proxiedMock);
                }
                else
                {
                    Type genericMockType = typeof(Mock<>).MakeGenericType(parameterType);
                    var mock = Activator.CreateInstance(genericMockType);
                    dependenciesList.Add(((Mock)mock).Object);

                }
            }
        }
        return dependenciesList.Any() ? dependenciesList.ToArray() : null;
    }
}
