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

namespace MicroTodo.IntegrationTests.Features.TodoItemEndpoints;

[Binding, Scope(Feature = "Update todo item endpoint")]
public class UpdateTodoItemEndpointSteps : BaseEndpointSteps, IClassFixture<TestWebApplicationFactory<Program>>
{
    public UpdateTodoItemEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory,
        ISpecFlowOutputHelper outputHelper
    ) : base(scenarioContext, factory, outputHelper)
    {
    }

    [Given(@"There is a todo item with id {long} exists")]
    public async Task GivenThereIsATodoItemWithIdExists(long todoItemId)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var todoItem = new TodoItemFaker()
            .WithId(todoItemId)
            .Generate();

        await dbContext.TodoItems.AddAsync(todoItem);
        await dbContext.SaveChangesAsync(default);
    }

    [Given(@"There is a todo group with id {long} exists")]
    public async Task WhenThereIsATodoGroupWithIdExists(long todoGroupId)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var todoGroup = new TodoGroupFaker()
            .WithId(todoGroupId)
            .Generate();

        await dbContext.TodoGroups.AddAsync(todoGroup);
        await dbContext.SaveChangesAsync(default);
    }

    [When(@"I send a PUT request to {string} with the todo item data")]
    public async Task WhenISendAPutRequestToWithTheTodoItemData(string url)
    {
        var todoItem = new Faker<UpdateTodoItemCommand>()
            .RuleFor(x => x.Title, f => f.Lorem.Sentence())
            .RuleFor(x => x.State, f => f.PickRandom<TodoItemState>())
            .RuleFor(x => x.DueDate, f => f.Date.Future().OrNull(f, 0.5f))
            .Generate();

        var response = await _httpClient.PutAsJsonAsync(url, todoItem);

        _scenarioContext.Set(response);
        _scenarioContext.Set(todoItem);
    }

    [When("I send a PUT request to {string} with the todo item with group id {long} data")]
    public async Task WhenISendAPutRequestToWithTheTodoItemWithGroupIdData(string url, long todoGroupId)
    {
        var todoItem = new Faker<UpdateTodoItemCommand>()
            .RuleFor(x => x.Title, f => f.Lorem.Sentence())
            .RuleFor(x => x.State, f => f.PickRandom<TodoItemState>())
            .RuleFor(x => x.DueDate, f => f.Date.Future().OrNull(f, 0.5f))
            .RuleFor(x => x.GroupId, todoGroupId)
            .Generate();

        var response = await _httpClient.PutAsJsonAsync(url, todoItem);

        _scenarioContext.Set(response);
        _scenarioContext.Set(todoItem);
    }

    [When("I send a PUT request to {string} with invalid data")]
    public async Task WhenISendAPutRequestToWithInvalidData(string url)
    {
        var todoItem = new Faker<UpdateTodoItemCommand>()
            .RuleFor(x => x.Title, _ => string.Empty)
            .RuleFor(x => x.State, f => f.PickRandom<TodoItemState>())
            .RuleFor(x => x.DueDate, f => f.Date.Future().OrNull(f, 0.5f))
            .RuleFor(x => x.GroupId, f => f.Random.Long(1000, 10000))
            .Generate();

        var response = await _httpClient.PutAsJsonAsync(url, todoItem);

        _scenarioContext.Set(response);
    }

    [When("I send a PUT request to {string} with the todo item data and unknown group id")]
    public async Task WhenISendAPutRequestToWithTheTodoItemDataWithInvalidGroupId(string url)
    {
        var todoItem = new Faker<UpdateTodoItemCommand>()
            .RuleFor(x => x.Title, f => f.Lorem.Sentence())
            .RuleFor(x => x.State, f => f.PickRandom<TodoItemState>())
            .RuleFor(x => x.DueDate, f => f.Date.Future().OrNull(f, 0.5f))
            .RuleFor(x => x.GroupId, f => f.Random.Long(1000, 10000))
            .Generate();

        var response = await _httpClient.PutAsJsonAsync(url, todoItem);

        _scenarioContext.Set(response);
    }

    [Then(@"the response status code should be {int}")]
    public void ThenTheResponseStatusCodeShouldBe(int httpStatus)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();

        response.StatusCode.Should().Be((HttpStatusCode)httpStatus);
    }

    [Then("the response body should contain the updated todo item with id {long}")]
    public async Task ThenTheResponseBodyShouldContainTheUpdatedTodoItem(long todoItemId)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();
        var todoItem = await response.Content.ReadFromJsonAsync<TodoItem>();

        todoItem.Should().NotBeNull()
                .And.Match<TodoItem>(x => x.Id == todoItemId)
                .And.BeEquivalentTo(_scenarioContext.Get<UpdateTodoItemCommand>(), options => options
                    .Excluding(x => x.Id)
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(100))).WhenTypeIs<DateTime>());
    }

    [Then("the database should contain a todo item with id {long} and updated data")]
    public async Task ThenTheDatabaseShouldContainATodoItemWithIdAndUpdatedData(long todoItemId)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var todoItem = await dbContext.TodoItems
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == todoItemId, default);

        todoItem.Should().NotBeNull()
                .And.Match<TodoItem>(x => x.Id == todoItemId)
                .And.BeEquivalentTo(_scenarioContext.Get<UpdateTodoItemCommand>(), options => options
                    .Excluding(x => x.Id)
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(100))).WhenTypeIs<DateTime>());
    }
}

