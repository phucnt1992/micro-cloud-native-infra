using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MicroTodo.IntegrationTests.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RemoveDbContext<T>(this IServiceCollection services) where T : DbContext
    {
        var dbContextFactoryDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextFactory<T>));

        if (dbContextFactoryDescriptor != null)
        {
            services.Remove(dbContextFactoryDescriptor);
        }

        var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));

        if (dbContextDescriptor != null)
        {
            services.Remove(dbContextDescriptor);
        }

        return services;
    }
}
