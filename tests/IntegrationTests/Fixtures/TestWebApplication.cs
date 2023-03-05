using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Extensions;
namespace MicroTodo.IntegrationTests.Fixtures
{
    public class TestWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
    {
        private readonly PostgreSqlTestcontainer _container;

        private string ConnectionString => _container.ConnectionString;

        [Obsolete("This constructor is only for the test framework.")]
        public TestWebApplicationFactory()
        {
            _container = new ContainerBuilder<PostgreSqlTestcontainer>()
                .WithDatabase(new PostgreSqlTestcontainerConfiguration
                {
                    Database = $"test_db_{Guid.NewGuid()}",
                    Username = "postgres",
                    Password = "postgres",
                    Port = new Random().Next(49152, 65535)
                })
                .WithImage("postgres:15-alpine")
                .WithCleanUp(true)
                .Build();
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services => services
                    .RemoveDbContext<ApplicationDbContext>()
                    .AddPooledDbContextFactory<ApplicationDbContext>(options =>
                        options.UseNpgsql(ConnectionString)));

            return base.CreateHost(builder);
        }

        public new async Task DisposeAsync()
        {
            await _container.DisposeAsync();
            await base.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
        }
    }
}
