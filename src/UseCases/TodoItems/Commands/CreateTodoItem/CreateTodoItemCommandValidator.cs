namespace MicroTodo.UseCases.TodoItems.Commands;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateTodoItemCommandValidator(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;

        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.State).NotNull();

        RuleFor(x => x.GroupId)
            .MustAsync(async (id, cancellationToken) =>
                id is null ||
                (id > 0 && await _dbContext.TodoGroups
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == id, cancellationToken)))
            .WithMessage(Constants.TodoGroupIdDoesNotExist);
    }
}
