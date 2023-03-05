namespace MicroTodo.UseCases.TodoItems.Commands;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand, TodoItem>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateTodoItemCommandHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<TodoItem> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.TodoItems
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        EntityNotFoundException.ThrowIfNull(entity, request.Id);

        entity.Title = request.Name;
        entity.State = request.State;
        entity.DueDate = request.DueDate;
        entity.GroupId = request.GroupId;
        entity.Version = Guid.NewGuid();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
