using System.Net;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Fixtures;

namespace MicroTodo.IntegrationTests.Steps;

public abstract class BaseEndpointSteps
{
    protected readonly ScenarioContext _scenarioContext;
    protected readonly TestWebApplicationFactory<Program> _factory;
    protected readonly HttpClient _httpClient;
    protected readonly ISpecFlowOutputHelper _outputHelper;

    protected BaseEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory,
        ISpecFlowOutputHelper outputHelper
    )
    {
        ArgumentNullException.ThrowIfNull(scenarioContext);
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(outputHelper);

        _scenarioContext = scenarioContext;
        _factory = factory;
        _httpClient = factory.CreateDefaultClient();
        _outputHelper = outputHelper;
    }

    [BeforeScenario]
    public async Task BeforeScenarioAsync()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        _outputHelper.WriteLine($"Connection String: {dbContext.Database.GetDbConnection().ConnectionString}");

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    [AfterScenario]
    public async Task AfterScenarioAsync()
    {
        using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
    }
}
