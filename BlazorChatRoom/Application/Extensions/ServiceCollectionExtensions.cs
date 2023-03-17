
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
       services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
    
}