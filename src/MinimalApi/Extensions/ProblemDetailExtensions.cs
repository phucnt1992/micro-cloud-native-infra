using FluentValidation;

using Microsoft.AspNetCore.Http.HttpResults;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Extensions;

namespace MicroTodo.MinimalApi.Extensions;

public static class ProblemDetailExtensions
{
    public static ProblemHttpResult ToNotFoundProblem(this EntityNotFoundException exception)
        => TypedResults.Problem(exception.Message, statusCode: StatusCodes.Status404NotFound);

    public static ProblemHttpResult ToConflictProblem(this EntityAlreadyExistsException exception)
        => TypedResults.Problem(exception.Message, statusCode: StatusCodes.Status409Conflict);

    public static ValidationProblem ToValidationProblem(this ValidationException exception)
        => TypedResults.ValidationProblem(exception.ToDictionary());

    public static IResult ToProblem(this Exception exception) => exception switch
    {
        EntityNotFoundException ex => ex.ToNotFoundProblem(),
        EntityAlreadyExistsException ex => ex.ToConflictProblem(),
        ValidationException ex => ex.ToValidationProblem(),
        _ => throw exception
    };
}
