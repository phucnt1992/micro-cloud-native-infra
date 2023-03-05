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

        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.GroupId)
            .MustAsync(async (id, cancellationToken) =>
                id is null ||
                await _dbContext.TodoGroups
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == id, cancellationToken))
            .WithMessage("The todo group does not exist.");
    }
}
