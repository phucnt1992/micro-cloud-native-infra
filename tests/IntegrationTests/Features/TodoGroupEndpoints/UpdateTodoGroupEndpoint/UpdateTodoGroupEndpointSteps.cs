using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MicroTodo.Domain.Entities;
using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Fixtures;
using MicroTodo.IntegrationTests.Steps;
using MicroTodo.UseCases.Commands;

using TechTalk.SpecFlow.Assist;

namespace MicroTodo.IntegrationTests.Features.TodoGroupEndpoints.UpdateTodoGroupEndpoint;

[Binding, Scope(Feature = "Update todo group endpoint")]
public class UpdateTodoGroupEndpointSteps : BaseEndpointSteps, IClassFixture<TestWebApplicationFactory<Program>>
{
    public UpdateTodoGroupEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory,
        ISpecFlowOutputHelper outputHelper
     ) : base(scenarioContext, factory, outputHelper) { }

    [Given("the following todo groups:")]
    public async Task GivenTheFollowingTodoGroupsAsync(Table table)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
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

    [When("I send a PUT request to {string} with the following data:")]
    public async Task ISendAPutRequestToUrlWithTheFollowingData(string url, Table table)
    {
        var todoGroup = table.CreateInstance<UpdateTodoGroupCommand>();

        var response = await _httpClient.PutAsJsonAsync(url, todoGroup);

        _scenarioContext.Set(response);
    }

    [Then("the response status code should be {int}")]
    public void ThenTheResponseStatusCodeShouldBe(int statusCode)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();
        response.StatusCode.Should().Be((HttpStatusCode)statusCode);
    }

    [Then("the response should contain the updated todo group detail:")]
    public async Task ThenTheResponseShouldContainTheUpdatedDetailInJson(Table table)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();
        var todoGroup = await response.Content.ReadFromJsonAsync<TodoGroupEntity>();

        todoGroup.Should().BeEquivalentTo(table.CreateInstance<TodoGroupEntity>(),
            options => options
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.ModifiedOn)
                .Excluding(x => x.Version)
                .Excluding(x => x.TodoList)
        );
    }

    [Then("the database should contain the following todo groups in any order:")]
    public async Task ThenTheDatabaseShouldContainTheFollowingInAnyOrder(Table table)
    {
        using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var actualTodoGroups = await dbContext.TodoGroups.AsNoTracking().ToListAsync();

        actualTodoGroups.Should().BeEquivalentTo(table.CreateSet<TodoGroupEntity>(),
            options => options
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.ModifiedOn)
                .Excluding(x => x.TodoList)
                .Excluding(x => x.Version)
        );
    }
}
