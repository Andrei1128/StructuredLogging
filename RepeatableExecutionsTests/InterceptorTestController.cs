using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;

namespace InterceptorTestController
{
    [ApiController]
    [Route("[controller]")]
    public class InterceptorTestController : ControllerBase
    {
        [HttpPost]
        public void Test()
        {
            var generator = new ProxyGenerator();
            var interceptor = new Interceptor();

            Calculator calculator = new Calculator();
            Type calculatorType = typeof(Calculator);

            ICalculator calculatorInterface = new Calculator();
            Type calculatorInterfaceType = typeof(ICalculator);

            var classProxyWithTarget = generator.CreateClassProxyWithTarget(calculator, interceptor);
            Console.WriteLine(nameof(classProxyWithTarget));
            Console.WriteLine(classProxyWithTarget.GetType());
            Console.WriteLine(classProxyWithTarget.Add(1, 1) + "\n");

            var classProxyWithoutTarget = generator.CreateClassProxy(calculatorType, interceptor);
            Console.WriteLine(nameof(classProxyWithoutTarget));
            Console.WriteLine(classProxyWithoutTarget.GetType());
            Console.WriteLine((classProxyWithoutTarget as Calculator)!.Add(1, 1) + "\n");

            var interfaceProxyWithTarget = generator.CreateInterfaceProxyWithTarget(calculatorInterface, interceptor);
            Console.WriteLine(nameof(interfaceProxyWithTarget));
            Console.WriteLine(interfaceProxyWithTarget.GetType());
            Console.WriteLine(interfaceProxyWithTarget.Add(1, 1) + "\n");

            var interfaceProxyWithoutTarget = generator.CreateInterfaceProxyWithoutTarget(calculatorInterfaceType, interceptor);
            Console.WriteLine(nameof(interfaceProxyWithoutTarget));
            Console.WriteLine(interfaceProxyWithoutTarget.GetType());
            Console.WriteLine((interfaceProxyWithoutTarget as ICalculator)!.Add(1, 1) + "\n");

            var interfaceProxyWithTargetInterface = generator.CreateInterfaceProxyWithTargetInterface(calculatorInterface, interceptor);
            Console.WriteLine(nameof(interfaceProxyWithTargetInterface));
            Console.WriteLine(interfaceProxyWithTargetInterface.GetType());
            Console.WriteLine(interfaceProxyWithTargetInterface.Add(1, 1) + "\n");
        }
    }
    public class Interceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Interceptor triggered!");
            invocation.ReturnValue = 3;
        }
    }
    public class Calculator : ICalculator
    {
        public virtual int Add(int a, int b)
        {
            return a + b;
        }
    }
    public interface ICalculator
    {
        int Add(int a, int b);
    }
}
