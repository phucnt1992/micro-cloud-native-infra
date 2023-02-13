using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using MicroTodo.Domain.Entities;
using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Fixtures;

using TechTalk.SpecFlow.Assist;

namespace MicroTodo.IntegrationTests.Features.TodoGroupEndpoints;

[Binding]
public class GetAllTodoGroupEndpointSteps : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly ScenarioContext _scenarioContext;
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public GetAllTodoGroupEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory
 )
    {
        ArgumentNullException.ThrowIfNull(scenarioContext);
        ArgumentNullException.ThrowIfNull(factory);

        _scenarioContext = scenarioContext;
        _factory = factory;
        _httpClient = factory.CreateDefaultClient();
    }

    [BeforeScenario]
    public async Task BeforeScenarioAsync()
    {
        using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

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

    [Given("the following todo groups:")]
    public async Task GivenTheFollowingTodoGroupsAsync(Table table)
    {
        using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var todoGroups = table.Rows.Select(row => new TodoGroupEntity
        {
            Id = long.Parse(row["Id"]),
            Name = row["Name"]
        })
        .ToList();

        await dbContext.TodoGroups.AddRangeAsync(todoGroups);
        await dbContext.SaveChangesAsync(default);
    }

    [When(@"I send a GET request to ""(.*)""")]
    public async Task ISendAGetRequestToUrl(string url)
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<TodoGroupEntity>>(url);

        _scenarioContext.Set(response);
    }

    [Then("the response should contain the following todo groups in any order:")]
    public void ThenTheResponseShouldContainTheFollowingInAnyOrder(Table table)
    {
        var response = _scenarioContext.Get<IEnumerable<TodoGroupEntity>>();

        response.Should().BeEquivalentTo(table.CreateSet<TodoGroupEntity>(),
            options => options
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.ModifiedOn)
        );
    }
}
