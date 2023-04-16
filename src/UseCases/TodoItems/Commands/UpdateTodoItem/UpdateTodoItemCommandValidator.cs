namespace MicroTodo.UseCases.TodoItems.Commands;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateTodoItemCommandValidator(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x
            .Title).NotEmpty();

        RuleFor(x => x
            .State).NotNull();

        RuleFor(x => x.GroupId)
            .MustAsync(async (id, cancellationToken) =>
                id is null ||
                (id > 0 && await _dbContext.TodoGroups
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == id, cancellationToken)))
            .WithMessage(Constants.TodoGroupIdDoesNotExist);
    }
}
