using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Commands;

public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, TodoEntity>
{
    private readonly IMicroTodoDbContext _dbContext;

    public UpdateTodoCommandHandler(IMicroTodoDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<TodoEntity> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.TodoList
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new EntityNotFoundException(nameof(TodoEntity), request.Id);

        entity.Title = request.Name;
        entity.State = request.State;
        entity.DueDate = request.DueDate;
        entity.GroupId = request.GroupId;
        entity.Version = Guid.NewGuid();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
