using System.Net;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using MicroTodo.Domain.Entities;
using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Fixtures;
using MicroTodo.IntegrationTests.Steps;

using TechTalk.SpecFlow.Assist;

namespace MicroTodo.IntegrationTests.Features.TodoGroupEndpoints.DeleteTodoGroupEndpoint;

[Binding, Scope(Feature = "Delete todo group endpoint")]
public class DeleteTodoGroupEndpointSteps : BaseEndpointSteps, IClassFixture<TestWebApplicationFactory<Program>>
{
    public DeleteTodoGroupEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory,
        ISpecFlowOutputHelper outputHelper
    ) : base(scenarioContext, factory, outputHelper)
    {
    }

    [Given("the following todo groups:")]
    public async Task GivenTheFollowingTodoGroupsAsync(Table table)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
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

    [When("I send a DELETE request to {string}")]
    public async Task ISendADeleteRequestToUrl(string url)
    {
        var response = await _httpClient.DeleteAsync(url);

        _scenarioContext.Set(response);
    }

    [Then("the response status code should be {int}")]
    public void ThenTheResponseStatusCodeShouldBe(int httpStatus)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();

        response.StatusCode.Should().Be((HttpStatusCode)httpStatus);
    }

    [Then("the database should contain the following todo groups in any order:")]
    public void ThenTheDatabaseShouldContainTheFollowingInAnyOrder(Table table)
    {
        using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var todoGroups = dbContext.TodoGroups.ToList();

        todoGroups.Should().BeEquivalentTo(table.CreateSet<TodoGroup>(),
            options => options
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.ModifiedOn)
        );
    }
}
