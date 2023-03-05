using System.Globalization;
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

[Binding, Scope(Feature = "Get todo items by group id endpoint")]
public class GetTodoItemsByGroupIdEndpointSteps : BaseEndpointSteps, IClassFixture<TestWebApplicationFactory<Program>>
{

    public GetTodoItemsByGroupIdEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory,
        ISpecFlowOutputHelper outputHelper
    ) : base(scenarioContext, factory, outputHelper)
    {
    }

    [Given("the following todo groups exist:")]
    public async Task GivenTheFollowingTodoGroupsExistAsync(Table table)
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

    [Given("the following todo items exist:")]
    public async Task GivenTheFollowingTodoItemsExistAsync(Table table)
    {
        using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var todoItems = table.Rows.Select(row => new TodoItem
        {
            Id = long.Parse(row["Id"]),
            Title = row["Title"],
            DueDate = DateTime.TryParse(row["DueDate"], null, DateTimeStyles.AssumeUniversal, out var dueDate)
                ? dueDate.ToUniversalTime()
                : null,
            State = Enum.TryParse<TodoItemState>(row["State"], out var state)
                ? state
                : TodoItemState.NotStarted,
            GroupId = long.TryParse(row["GroupId"], out var groupId)
                ? groupId
                : null,
        })
        .ToList();

        await dbContext.TodoItems.AddRangeAsync(todoItems);
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

    [Then("the response should contain the following in any order:")]
    public async Task ThenTheResponseShouldContainTheFollowingTodoItems(Table table)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();

        var todoItems = await response.Content.ReadFromJsonAsync<IEnumerable<TodoItem>>();

        todoItems.Should().BeEquivalentTo(table.CreateSet<TodoItem>(), options => options
            .Excluding(x => x.CreatedOn)
            .Excluding(x => x.ModifiedOn));
    }
}
