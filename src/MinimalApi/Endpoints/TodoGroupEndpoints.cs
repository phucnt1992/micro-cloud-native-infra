namespace MicroTodo.MinimalApi.Endpoints;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using MicroTodo.MinimalApi.Extensions;
using MicroTodo.UseCases.Commands;
using MicroTodo.UseCases.Queries;

public static class TodoGroupEndpoints
{
    public static RouteGroupBuilder MapTodoGroupsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllTodoGroups);

        group.MapGet("/{id}", GetTodoGroupById)
            .WithName(nameof(GetTodoGroupById));

        group.MapPost("/", PostTodoGroup);

        group.MapPut("/{id}", PutTodoGroup);

        group.MapDelete("/{id}", DeleteTodoGroupById);

        group.MapGet("/{id}/todo", GetTodoListByTodoGroupId);

        return group;
    }

    static async Task<IResult> GetAllTodoGroups(
        [FromServices] IMediator mediator)
    {
        var todoGroups = await mediator.Send(new GetAllTodoGroupsQuery());

        return TypedResults.Ok(todoGroups);
    }

    static async Task<IResult> GetTodoGroupById(
        [FromRoute] long id,
        [FromServices] IMediator mediator
    )
    {
        try
        {
            var entity = await mediator.Send(new GetTodoGroupDetailByIdQuery { Id = id });

            return TypedResults.Ok(entity);
        }
        catch (Exception ex)
        {
            return ex.ToProblem();
        }
    }

    static async Task<IResult> PostTodoGroup(
        [FromServices] IMediator mediator,
        [FromBody] CreateTodoGroupCommand command)
    {
        try
        {
            var id = await mediator.Send(command);

            return TypedResults.CreatedAtRoute(id, nameof(GetTodoGroupById), new { id });
        }
        catch (Exception ex)
        {
            return ex.ToProblem();
        }
    }

    static async Task<IResult> PutTodoGroup(
        [FromRoute] long id,
        [FromServices] IMediator mediator,
        [FromBody] UpdateTodoGroupCommand command)
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

    static async Task<IResult> DeleteTodoGroupById(
        [FromRoute] long id,
        [FromServices] IMediator mediator
        )
    {
        try
        {
            await mediator.Send(new DeleteTodoGroupByIdCommand { Id = id });

            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return ex.ToProblem();
        }
    }

    static async Task<IResult> GetTodoListByTodoGroupId(
        [FromRoute] long id,
        [FromServices] IMediator mediator
    )
    {
        var todoList = await mediator.Send(new GetTodoListByGroupIdQuery { GroupId = id });

        return TypedResults.Ok(todoList);
    }
}
