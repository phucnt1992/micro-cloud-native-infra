using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MicroTodo.Domain.Entities;
using MicroTodo.Infra.Persistence;
using MicroTodo.IntegrationTests.Fixtures;
using MicroTodo.IntegrationTests.Steps;
using MicroTodo.UseCases.TodoGroups.Commands;

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

        var todoGroups = table.Rows.Select(row => new TodoGroup
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

        var responseContent = await response.Content.ReadAsStringAsync();

        _outputHelper.WriteLine(responseContent);

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
        var todoGroup = await response.Content.ReadFromJsonAsync<TodoGroup>();

        todoGroup.Should().BeEquivalentTo(table.CreateInstance<TodoGroup>(),
            options => options
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.ModifiedOn)
                .Excluding(x => x.Version)
                .Excluding(x => x.TodoList)
        );

        _scenarioContext.Set(todoGroup);
    }

    [Then("the database should contain the following todo groups in any order:")]
    public async Task ThenTheDatabaseShouldContainTheFollowingInAnyOrder(Table table)
    {
        using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var actualTodoGroups = await dbContext.TodoGroups.AsNoTracking().ToListAsync();

        actualTodoGroups.Should().BeEquivalentTo(table.CreateSet<TodoGroup>(),
            options => options
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.ModifiedOn)
                .Excluding(x => x.TodoList)
                .Excluding(x => x.Version)
        );
    }

    [Then("the modified date should be greater than the created date for updated todo group record")]
    public void ThenTheModifiedDateShouldBeGreaterThanTheCreatedDateForEachRecord()
    {
        var todoGroup = _scenarioContext.Get<TodoGroup>();

        todoGroup.ModifiedOn.Should().BeAfter(todoGroup.CreatedOn);
    }
}
