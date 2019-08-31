using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace GeobaseWebApp.Utils
{
    public static class DependencyLocator
    {
        public static TSvc Get<TSvc>(string service)
        {
            var initKey = ConfigurationManager.AppSettings[$"{service}InitKey"];
            var implementation = ConfigurationManager.AppSettings[$"{service}Impl"];

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(x => x.ExportedTypes);
            var impl = types.FirstOrDefault(t => t.FullName == implementation);
            if (impl == null)
            {
                throw new Exception($"Implementation for {service} not found");
            }

            var factory = impl.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(MatchesFactoryMethod<TSvc>);
            if (factory == null)
            {
                throw new Exception("Implementation does not have public factory method");
            }

            return (TSvc)factory.Invoke(null, new object[] {initKey});
        }

        private static bool MatchesFactoryMethod<TSvc>(MethodInfo m)
        {
            return m.Name == "Load" 
                   && typeof(TSvc).IsAssignableFrom(m.ReturnParameter?.ParameterType)
                   && m.GetParameters().Length == 1
                   && m.GetParameters()[0].ParameterType == typeof(string);
        }
    }
}