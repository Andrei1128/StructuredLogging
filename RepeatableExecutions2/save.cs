//using System;
//using System.Linq;
//using System.Reflection;
//using System.Reflection.Emit;

//// Aspect interface
//public interface IAspect
//{
//    void BeforeMethodCall(string methodName);
//    void AfterMethodCall(string methodName);
//}

//// Global aspect decorator
//public class GlobalAspectDecorator : IAspect
//{
//    public void BeforeMethodCall(string methodName)
//    {
//        Console.WriteLine($"Before {methodName}.");
//    }

//    public void AfterMethodCall(string methodName)
//    {
//        Console.WriteLine($"After {methodName}.");
//    }
//}

//// Aspect attribute
//[AttributeUsage(AttributeTargets.Method)]
//public class ApplyGlobalAspectAttribute : Attribute { }

//// Aspect applier
//public class AspectApplier
//{
//    private static readonly IAspect GlobalAspect = new GlobalAspectDecorator();

//    public static T ApplyGlobalAspect<T>(T target)
//    {
//        Type targetType = target.GetType();
//        Type proxyType = CreateProxyType(targetType);

//        return (T)Activator.CreateInstance(proxyType, new[] { target });
//    }

//    private static Type CreateProxyType(Type targetType)
//    {
//        AppDomain currentDomain = AppDomain.CurrentDomain;
//        AssemblyName assemblyName = new AssemblyName($"AspectProxy_{targetType.Name}");
//        AssemblyBuilder assemblyBuilder = currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
//        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
//        TypeBuilder typeBuilder = moduleBuilder.DefineType(
//            $"Proxy_{targetType.Name}",
//            TypeAttributes.Public | TypeAttributes.Class,
//            typeof(object),
//            new[] { targetType });

//        foreach (MethodInfo method in targetType.GetMethods())
//        {
//            bool applyAspect = method.GetCustomAttributes(typeof(ApplyGlobalAspectAttribute), true).Any();

//            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
//                method.Name,
//                MethodAttributes.Public | MethodAttributes.Virtual,
//                method.ReturnType,
//                method.GetParameters().Select(p => p.ParameterType).ToArray());

//            ILGenerator il = methodBuilder.GetILGenerator();

//            if (applyAspect)
//            {
//                il.Emit(OpCodes.Ldarg_0);
//                il.Emit(OpCodes.Callvirt, typeof(IAspect).GetMethod("BeforeMethodCall"));
//            }

//            il.Emit(OpCodes.Ldarg_0);
//            il.Emit(OpCodes.Call, method);
//            il.Emit(OpCodes.Stloc_0);

//            if (applyAspect)
//            {
//                il.Emit(OpCodes.Ldarg_0);
//                il.Emit(OpCodes.Callvirt, typeof(IAspect).GetMethod("AfterMethodCall"));
//            }

//            il.Emit(OpCodes.Ldloc_0);
//            il.Emit(OpCodes.Ret);
//        }

//        return typeBuilder.CreateType();
//    }
//}

//// Target class
//public class TargetClass
//{
//    [ApplyGlobalAspect]
//    public void Method1()
//    {
//        Console.WriteLine("Method1 executed.");
//    }

//    public void Method2()
//    {
//        Console.WriteLine("Method2 executed.");
//    }
//}

//class Program
//{
//    static void Main(string[] args)
//    {
//        var target = new TargetClass();
//        var decoratedTarget = AspectApplier.ApplyGlobalAspect(target);

//        decoratedTarget.Method1();
//        decoratedTarget.Method2();
//    }
//}
