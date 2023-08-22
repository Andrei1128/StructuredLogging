//using Microsoft.AspNetCore.Builder;
//using StructuredLogging.Attributes;
//using System.Reflection;

//namespace StructuredLogging.Service
//{
//    public static class AddingInvocation
//    {
//        public static void ApplyProxyBehaviorToMethods(IApplicationBuilder app, Type Program)
//        {
//            var assembly = Program.Assembly;
//            var types = assembly.GetTypes();

//            foreach (var type in types)
//            {
//                var methods = type.GetMethods();

//                foreach (var method in methods)
//                {
//                    if (method.GetCustomAttribute(typeof(LogAttribute), false) != null)
//                    {
//                        var instance = ActivatorUtilities.CreateInstance(app.ApplicationServices, type);

//                        var methodDelegate = (Action)Delegate.CreateDelegate(typeof(Action), instance, method);

//                        Action proxyMethod = () =>
//                        {
//                            ProxyDecorator.ExecuteWithProxy(methodDelegate);
//                        };

//                        proxyMethod.Invoke();
//                    }
//                }
//            }
//        }
//    }
//}
