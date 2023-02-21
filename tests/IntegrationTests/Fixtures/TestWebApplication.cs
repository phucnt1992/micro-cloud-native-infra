using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Extensions;

namespace MicroTodo.IntegrationTests.Fixtures
{
    public class TestWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services => services
                    .RemoveDbContext()
                    .AddPooledDbContextFactory<ApplicationDbContext>(options =>
                        options.UseSqlite($"Data Source=test_MicroTodo_{Guid.NewGuid()}.db")));

            return base.CreateHost(builder);
        }
    }
}
