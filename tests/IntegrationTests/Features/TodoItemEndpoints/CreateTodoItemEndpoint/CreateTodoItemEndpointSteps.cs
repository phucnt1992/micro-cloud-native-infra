using System.Net;
using System.Net.Http.Json;

using Bogus;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MicroTodo.Domain.Entities;
using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Fakers;
using MicroTodo.IntegrationTests.Fixtures;
using MicroTodo.IntegrationTests.Steps;
using MicroTodo.UseCases.TodoItems.Commands;

using TechTalk.SpecFlow.Assist;

namespace MicroTodo.IntegrationTests.Features.TodoEndpoints;

[Binding, Scope(Feature = "Create todo item endpoint")]
public class CreateTodoItemEndpointSteps : BaseEndpointSteps, IClassFixture<TestWebApplicationFactory<Program>>
{
    public CreateTodoItemEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory,
        ISpecFlowOutputHelper outputHelper
    ) : base(scenarioContext, factory, outputHelper)
    {
    }

    [Given(@"There is no todo item with id {long} in database")]
    public async Task GivenThereIsNoTodoItemWithIdInDatabase(long todoItemId)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var isAnyId = await dbContext.TodoItems
            .AsNoTracking()
            .AnyAsync(x => x.Id == todoItemId, default);

        isAnyId.Should().BeFalse();
    }

    [Given(@"I have a todo group with id {long}")]
    public async Task GivenIHaveATodoGroupWithId(long todoGroupId)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var todoGroup = new TodoGroupFaker()
            .WithId(todoGroupId)
            .Generate();

        await dbContext.TodoGroups.AddAsync(todoGroup);
        await dbContext.SaveChangesAsync(default);
    }

    [When(@"I send a POST request to {string} with the todo item data")]
    public async Task WhenISendAPostRequestToThe(string url)
    {
        var todoItem = new Faker<CreateTodoItemCommand>()
            .RuleFor(x => x.Title, f => f.Lorem.Sentence())
            .RuleFor(x => x.State, f => f.PickRandom<TodoItemState>())
            .RuleFor(x => x.DueDate, f => f.Date.Future().OrNull(f, 0.5f))
            .Generate();

        var response = await _httpClient.PostAsJsonAsync(url, todoItem);

        _scenarioContext.Set(response);
        _scenarioContext.Set(todoItem);
    }

    [When(@"I send a POST request to {string} with the todo item data contain group id {long}")]
    public async Task WhenISendAPostRequestToTheWithTheTodoItemDataContainGroupId(
        string url,
        long todoGroupId
    )
    {
        var todoItem = new Faker<CreateTodoItemCommand>()
            .RuleFor(x => x.Title, f => f.Lorem.Sentence())
            .RuleFor(x => x.State, f => f.PickRandom<TodoItemState>())
            .RuleFor(x => x.DueDate, f => f.Date.Future().OrNull(f, 0.5f))
            .RuleFor(x => x.GroupId, todoGroupId)
            .Generate();

        var response = await _httpClient.PostAsJsonAsync(url, todoItem);

        _scenarioContext.Set(response);
        _scenarioContext.Set(todoItem);
    }

    [When(@"I send a POST request to {string} with the invalid todo item data")]
    public async Task WhenISendAPostRequestToTheWithTheInvalidTodoItemData(string url)
    {
        var todoItem = new Faker<CreateTodoItemCommand>()
            .RuleFor(x => x.State, f => f.PickRandom<TodoItemState>())
            .RuleFor(x => x.DueDate, f => f.Date.Future().OrNull(f, 0.5f))
            .RuleFor(x => x.GroupId, f => f.Random.Long(1, 100));

        var response = await _httpClient.PostAsJsonAsync(url, todoItem);

        _scenarioContext.Set(response);
        _scenarioContext.Set(todoItem);
    }

    [Then(@"the response status code should be {int}")]
    public void ThenTheResponseStatusCodeShouldBe(int httpStatus)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();

        response.StatusCode.Should().Be((HttpStatusCode)httpStatus);
    }

    [Then("the response header contains {string} with value {string}")]
    public void ThenTheResponseHeaderContains(string headerName, string headerValue)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();
        response.Headers.Should()
            .Contain(x => x.Key.Equals(headerName))
            .And.Subject.Single(x => x.Key.Equals(headerName)).Value.Single()
            .Should().EndWith(headerValue);
    }

    [Then(@"the database should contain created todo item with id {long}")]
    public async Task ThenTheDatabaseShouldContainTodoItemWithId(long todoItemId)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var requestBody = _scenarioContext.Get<CreateTodoItemCommand>();

        var todoItem = await dbContext.TodoItems
            .AsNoTracking()
            .SingleAsync(x => x.Id == todoItemId, default);

        todoItem.Should().BeEquivalentTo(requestBody, options => options
            .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(100))).WhenTypeIs<DateTime>())
            .And.Match<TodoItem>(x => x.Id == todoItemId);
    }
}
