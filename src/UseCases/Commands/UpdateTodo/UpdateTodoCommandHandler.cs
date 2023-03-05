using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Commands;

public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, TodoItem>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateTodoCommandHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<TodoItem> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.TodoList
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new EntityNotFoundException(nameof(TodoItem), request.Id);

        entity.Title = request.Name;
        entity.State = request.State;
        entity.DueDate = request.DueDate;
        entity.GroupId = request.GroupId;
        entity.Version = Guid.NewGuid();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
