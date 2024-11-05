using System.Reflection;

namespace NewsApp.API.Atributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ServiceAttribute: Attribute
{
}


public static class ServiceRegistrationExtensions
{
    public static void AddServicesWithAttribute(this IServiceCollection services, Assembly assembly)
    {
        var typesWithAttribute = assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<ServiceAttribute>() != null);

        foreach (var type in typesWithAttribute)
        {
            services.AddScoped(type); 
        }
    }
}