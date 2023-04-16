using MediatR;

using Microsoft.AspNetCore.Mvc;

using MicroTodo.MinimalApi.Extensions;
using MicroTodo.UseCases.TodoItems.Commands;
using MicroTodo.UseCases.TodoItems.Queries;

namespace MicroTodo.MinimalApi.Endpoints;

public static class TodoItemEndpoint
{
    public static RouteGroupBuilder MapTodoItemEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", GetTodoItemById)
            .WithName(nameof(GetTodoItemById));

        group.MapPost("/", PostTodoItem);

        group.MapPut("/{id}", PutTodoItem);

        group.MapDelete("/{id}", DeleteTodoItemById);

        return group;
    }

    static async Task<IResult> GetTodoItemById(
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

    static async Task<IResult> PostTodoItem(
        [FromServices] IMediator mediator,
        [FromBody] CreateTodoItemCommand command)
    {
        try
        {
            var id = await mediator.Send(command);

            return TypedResults.CreatedAtRoute(id, nameof(GetTodoItemById), new { id });
        }
        catch (Exception ex)
        {
            return ex.ToProblem();
        }
    }

    static async Task<IResult> PutTodoItem(
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

    static async Task<IResult> DeleteTodoItemById(
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
