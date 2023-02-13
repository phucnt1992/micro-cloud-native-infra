using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MicroTodo.Infra.Persistence;

namespace MicroTodo.IntegrationTests.Fixtures
{
    public class TestWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextFactory<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddPooledDbContextFactory<ApplicationDbContext>(options => options.UseSqlite($"Data Source=test_MicroTodo_{Guid.NewGuid()}.db"));
            });

            return base.CreateHost(builder);
        }
    }
}
