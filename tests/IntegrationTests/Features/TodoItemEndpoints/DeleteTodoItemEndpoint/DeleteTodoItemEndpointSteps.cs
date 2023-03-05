using System.Net;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Fakers;
using MicroTodo.IntegrationTests.Fixtures;
using MicroTodo.IntegrationTests.Steps;

namespace MicroTodo.IntegrationTests.Features.TodoEndpoints;

[Binding, Scope(Feature = "Delete todo item endpoint")]
public class DeleteTodoItemEndpointSteps : BaseEndpointSteps, IClassFixture<TestWebApplicationFactory<Program>>
{
    public DeleteTodoItemEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory,
        ISpecFlowOutputHelper outputHelper
    ) : base(scenarioContext, factory, outputHelper)
    {
    }

    [Given("there are {int} todo items exist in database")]
    public async Task GivenTheTodoItemsExistInDatabase(int size)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var todoItems = new TodoItemFaker()
            .Generate(size);

        await dbContext.TodoItems.AddRangeAsync(todoItems);
        await dbContext.SaveChangesAsync(default);
    }

    [When(@"I send a DELETE request to {string}")]
    public async Task WhenISendADeleteRequestToThe(string url)
    {
        var response = await _httpClient.DeleteAsync(url);

        _scenarioContext.Set(response);
    }

    [Then(@"the response status code should be {int}")]
    public void ThenTheResponseStatusCodeShouldBe(int httpStatus)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();

        response.StatusCode.Should().Be((HttpStatusCode)httpStatus);
    }

    [Then("the todo item with id {long} should not exist in database")]
    public async Task ThenTheTodoItemWithIdShouldNotExistInDatabase(long todoItemId)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var isAnyId = await dbContext.TodoItems
            .AsNoTracking()
            .AnyAsync(x => x.Id == todoItemId, default);

        isAnyId.Should().BeFalse();
    }
}
