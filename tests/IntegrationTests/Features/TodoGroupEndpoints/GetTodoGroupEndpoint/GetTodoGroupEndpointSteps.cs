using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using MicroTodo.Domain.Entities;
using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Fixtures;
using MicroTodo.IntegrationTests.Steps;

using TechTalk.SpecFlow.Assist;

namespace MicroTodo.IntegrationTests.Features.TodoGroupEndpoints;

[Binding, Scope(Feature = "Get todo group endpoint")]
public class GetTodoGroupEndpointSteps : BaseEndpointSteps, IClassFixture<TestWebApplicationFactory<Program>>
{
    public GetTodoGroupEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory,
        ISpecFlowOutputHelper outputHelper
 ) : base(scenarioContext, factory, outputHelper) { }

    [Given("the following todo groups:")]
    public async Task GivenTheFollowingTodoGroupsAsync(Table table)
    {
        using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var todoGroups = table.Rows.Select(row => new TodoGroup
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
        var response = await _httpClient.GetAsync(url);

        _scenarioContext.Set(response);
    }

    [Then("the response status code should be {int}")]
    public void ThenTheResponseStatusCodeShouldBe(int statusCode)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();

        response.StatusCode.Should().Be((HttpStatusCode)statusCode);
    }

    [Then("the response should contain the todo group detail:")]
    public async Task ThenTheResponseShouldContainTheDetailInJson(Table table)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();

        var todoGroups = await response.Content.ReadFromJsonAsync<TodoGroup>();

        todoGroups.Should().BeEquivalentTo(table.CreateInstance<TodoGroup>(),
            options => options
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.ModifiedOn)
        );
    }
}
