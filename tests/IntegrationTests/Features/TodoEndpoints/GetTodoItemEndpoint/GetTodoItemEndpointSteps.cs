using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using MicroTodo.Domain.Entities;
using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Fakers;
using MicroTodo.IntegrationTests.Fixtures;
using MicroTodo.IntegrationTests.Steps;

namespace MicroTodo.IntegrationTests.Features.TodoEndpoints;

[Binding, Scope(Feature = "Get todo item endpoint")]
public class GetTodoItemEndpointSteps : BaseEndpointSteps, IClassFixture<TestWebApplicationFactory<Program>>
{
    public GetTodoItemEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory,
        ISpecFlowOutputHelper outputHelper
    ) : base(scenarioContext, factory, outputHelper)
    {
    }

    [Given(@"a todo item with id {int} exists")]
    public async Task GivenIHaveATodoItemWithId(int todoItemId)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var todoItem = new TodoItemFaker()
            .WithId(todoItemId)
            .Generate();

        await dbContext.TodoItems.AddAsync(todoItem);
        await dbContext.SaveChangesAsync(default);
    }

    [When(@"I send a GET request to {string}")]
    public async Task WhenISendAGetRequestToThe(string url)
    {
        var response = await _httpClient.GetAsync(url);

        _scenarioContext.Set(response);
    }

    [Then(@"the response status code should be {int}")]
    public void ThenTheResponseStatusCodeShouldBe(int httpStatus)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();

        response.StatusCode.Should().Be((HttpStatusCode)httpStatus);
    }

    [Then("the response body should contain todo item with id {int}")]
    public async Task ThenTheResponseBodyShouldContainTodoItemWithId(int todoItemId)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();
        var todoItem = await response.Content.ReadFromJsonAsync<TodoItem>();

        todoItem.Should().NotBeNull()
            .And.Match<TodoItem>(x => x.Id == todoItemId);
    }
}
