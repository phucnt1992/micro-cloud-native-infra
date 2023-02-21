using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using MicroTodo.Domain.Entities;
using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Fixtures;
using MicroTodo.IntegrationTests.Steps;

using TechTalk.SpecFlow.Assist;

namespace MicroTodo.IntegrationTests.Features.TodoGroupEndpoints;

[Binding, Scope(Feature = "get all todo group endpoint")]
public class GetAllTodoGroupEndpointSteps : BaseEndpointSteps, IClassFixture<TestWebApplicationFactory<Program>>
{
    public GetAllTodoGroupEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory,
        ISpecFlowOutputHelper outputHelper
    ) : base(scenarioContext, factory, outputHelper)
    {
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

    [When("I send a GET request to {string}")]
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
