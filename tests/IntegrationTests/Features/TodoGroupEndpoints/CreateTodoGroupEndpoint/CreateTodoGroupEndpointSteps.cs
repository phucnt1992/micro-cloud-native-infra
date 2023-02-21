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

namespace MicroTodo.IntegrationTests.Features.TodoGroupEndpoints.CreateTodoGroupEndpoint;

[Binding, Scope(Feature = "Create todo group endpoint")]
public class CreateTodoGroupEndpointSteps : BaseEndpointSteps
{
    public CreateTodoGroupEndpointSteps(
        ScenarioContext scenarioContext,
        TestWebApplicationFactory<Program> factory,
        ISpecFlowOutputHelper outputHelper) : base(scenarioContext, factory, outputHelper)
    {
    }

    [When("I send a POST request to {string} with following data:")]
    public async Task WhenISendAPostRequestToCreateANewTodoGroup(string url, Table table)
    {
        var response = await _httpClient.PostAsJsonAsync(url, table.CreateInstance<CreateTodoGroupCommand>());

        _scenarioContext.Set(response);
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

    [Then("the response status code should be {int}")]
    public void ThenTheResponseStatusCodeShouldBe(int statusCode)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();
        response.StatusCode.Should().Be((HttpStatusCode)statusCode);
    }

    [Then("the database contains the following todo groups:")]
    public async Task ThenTheDatabaseContainsTheFollowingTodoGroups(Table table)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var dataSet = await dbContext.TodoGroups
            .AsNoTracking()
            .ToListAsync();

        dataSet.Should().BeEquivalentTo(table.CreateSet<TodoGroupEntity>(),
            options => options
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.ModifiedOn)
                .Excluding(x => x.Version));
    }
}
