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
    public void ReplayFull()
    {
        string serializedLog = File.ReadAllText("C:\\Users\\Andrei\\Facultate\\C#\\RepeatableExecutionsTests\\Logging\\logs\\GetWeatherEndpoint-202311211829502569398");
        var jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        Log log = JsonConvert.DeserializeObject<Log>(serializedLog, jsonSettings);

        Type classType = Type.GetType(log.Entry.Class);

        var classInstance = CreateDependencies(log);
        if (classInstance != null)
        {
            var methodInfo = classType.GetMethod(log.Entry.Method);
            if (methodInfo != null)
            {
                Invoke(classInstance, methodInfo, log.Entry.Input);
            }
        }
    }
    public object CreateDependencies(Log log)
    {
        Type classType = Type.GetType(log.Entry.Class);

        List<object>? dependencies = new List<object>();

        foreach (var dep in log.Interactions)
        {
            dependencies.Add(CreateDependencies(dep));
        }

        ParameterInfo[] constructorParameters = GetConstructorParameters(classType);

        if (constructorParameters.Any())
        {
            var filteredConstructorParameters = from param in constructorParameters
                                                where !dependencies.Any(x => param.ParameterType.IsAssignableFrom(x.GetType()))
                                                select param;

            foreach (var parameter in filteredConstructorParameters)
            {
                Type parameterType = parameter.ParameterType;

                var classNameList = from _log in log.Interactions
                                    where parameterType.IsAssignableFrom(Type.GetType(_log.Entry.Class))
                                    select _log.Entry.Class;

                if (classNameList.Any())
                {
                    var mocksList = from _log in log.Interactions
                                    where _log.Entry.Class == classNameList.First()
                                    select new MockObject(_log.Id, _log.Entry.Method, _log.Exit.Output);

                    var mockInterceptor = new MockInterceptor(mocksList);
                    object? proxiedMock = proxyGenerator.CreateInterfaceProxyWithoutTarget(parameterType, mockInterceptor);
                    dependencies.Add(proxiedMock);
                }
                else
                {
                    Type genericMockType = typeof(Mock<>).MakeGenericType(parameterType);
                    var mock = Activator.CreateInstance(genericMockType);
                    dependencies.Add(((Mock)mock).Object);

                }
            }
        }



        if (dependencies.Any())
        {
            return Activator.CreateInstance(classType, dependencies.ToArray());
        }
        return Activator.CreateInstance(classType);
    }
    public void Replay(int id = 0)
    {
        string serializedLog = File.ReadAllText("C:\\Users\\Andrei\\Facultate\\C#\\RepeatableExecutionsTests\\Logging\\logs\\GetWeatherEndpoint-202311201909118155110");
        var jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        Log log = JsonConvert.DeserializeObject<Log>(serializedLog, jsonSettings);

        if (id != 0)
            log = FindNodeById(log, id);

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
    public Log FindNodeById(Log root, int id)
    {
        if (root.Id == id)
            return root;

        foreach (var child in root.Interactions)
        {
            var result = FindNodeById(child, id);
            if (result != null)
                return result;
        }

        return null;
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
                                    select new MockObject(log.Id, log.Entry.Method, log.Exit.Output);

                    var mockInterceptor = new MockInterceptor(mocksList);
                    object? proxiedMock = null;
                    proxiedMock = proxyGenerator.CreateInterfaceProxyWithoutTarget(parameterType, mockInterceptor);
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
