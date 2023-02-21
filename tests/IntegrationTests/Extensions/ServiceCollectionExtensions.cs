using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MicroTodo.Infra.Persistence;

namespace MicroTodo.IntegrationTests.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RemoveDbContext(this IServiceCollection services)
    {
        var dbContextFactoryDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextFactory<ApplicationDbContext>));

        if (dbContextFactoryDescriptor != null)
        {
            services.Remove(dbContextFactoryDescriptor);
        }

        var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

        if (dbContextDescriptor != null)
        {
            services.Remove(dbContextDescriptor);
        }

        return services;
    }
}
