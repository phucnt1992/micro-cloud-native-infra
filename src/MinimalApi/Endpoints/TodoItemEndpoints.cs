using MediatR;

using Microsoft.AspNetCore.Mvc;

using MicroTodo.MinimalApi.Extensions;
using MicroTodo.UseCases.TodoGroups.Queries;
using MicroTodo.UseCases.TodoItems.Commands;
using MicroTodo.UseCases.TodoItems.Queries;

namespace MicroTodo.MinimalApi.Endpoints;

public static class TodoItemEndpoint
{
    public static RouteGroupBuilder MapTodoItemEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", GetTodoById)
            .WithName(nameof(GetTodoById));

        group.MapPost("/", PostTodo);

        group.MapPut("/{id}", PutTodo);

        group.MapDelete("/{id}", DeleteTodoById);

        return group;
    }

    static async Task<IResult> GetTodoById(
        [FromServices] IMediator mediator,
        [FromRoute] long id)
    {
        try
        {
            var todo = await mediator.Send(new GetTodoItemByIdQuery { Id = id });

            return TypedResults.Ok(todo);
        }
        catch (Exception ex)
        {
            return ex.ToProblem();
        }
    }

    static async Task<IResult> PostTodo(
        [FromServices] IMediator mediator,
        [FromBody] CreateTodoItemCommand command)
    {
        try
        {
            var id = await mediator.Send(command);

            return TypedResults.CreatedAtRoute(id, nameof(GetTodoById), new { id });
        }
        catch (Exception ex)
        {
            return ex.ToProblem();
        }
    }

    static async Task<IResult> PutTodo(
        [FromServices] IMediator mediator,
        [FromRoute] long id,
        [FromBody] UpdateTodoItemCommand command)
    {
        try
        {
            var entity = await mediator.Send(command with { Id = id });

            return TypedResults.Ok(entity);
        }
        catch (Exception ex)
        {
            return ex.ToProblem();
        }
    }

    static async Task<IResult> DeleteTodoById(
        [FromServices] IMediator mediator,
        [FromRoute] long id)
    {
        try
        {
            await mediator.Send(new DeleteTodoItemByIdCommand { Id = id });

            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return ex.ToProblem();
        }
    }
}
