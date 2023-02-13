using MediatR;

using Microsoft.AspNetCore.Mvc;

using MicroTodo.UseCases.Commands;
using MicroTodo.UseCases.Queries;
namespace MicroTodo.MinimalApi.Endpoints;

public static class TodoEndpoint
{
    public static RouteGroupBuilder MapTodoEndpoints(this RouteGroupBuilder group)
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
        var todo = await mediator.Send(new GetTodoGroupDetailByIdQuery { Id = id });

        return TypedResults.Ok(todo);
    }

    static async Task<IResult> PostTodo(
        [FromServices] IMediator mediator,
        [FromBody] CreateTodoCommand command)
    {
        var id = await mediator.Send(command);

        return TypedResults.CreatedAtRoute(id, nameof(GetTodoById), new { id });
    }

    static async Task<IResult> PutTodo(
        [FromServices] IMediator mediator,
        [FromRoute] long id,
        [FromBody] UpdateTodoCommand command)
    {
        var entity = await mediator.Send(command with { Id = id });

        return TypedResults.Ok(entity);
    }

    static async Task<IResult> DeleteTodoById(
        [FromServices] IMediator mediator,
        [FromRoute] long id)
    {
        await mediator.Send(new DeleteTodoByIdCommand { Id = id });

        return TypedResults.NoContent();
    }
}
