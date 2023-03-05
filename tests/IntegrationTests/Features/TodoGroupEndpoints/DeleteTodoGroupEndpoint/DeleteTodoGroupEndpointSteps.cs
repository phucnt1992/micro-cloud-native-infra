using System.Globalization;
using System.Net;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
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

    [Given("the following todo items:")]
    public async Task GivenTheFollowingTodoItems(Table table)
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

    [Then("the database should contain the following todo items in any order:")]
    public async Task ThenTheDatabaseShouldContainTheTodoItem(Table table)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var todoItems = await dbContext.TodoItems
            .AsNoTracking()
            .ToListAsync();

        todoItems.Should().BeEquivalentTo(table.CreateSet<TodoItem>(),
            options => options
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.ModifiedOn)
        );
    }
}
