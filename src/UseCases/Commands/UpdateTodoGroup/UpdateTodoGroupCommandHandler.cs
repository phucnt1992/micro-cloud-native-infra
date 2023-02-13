using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Commands;

public class UpdateTodoGroupCommandHandler : IRequestHandler<UpdateTodoGroupCommand, TodoGroupEntity>
{
    private readonly IMicroTodoDbContext _dbContext;

    public UpdateTodoGroupCommandHandler(IMicroTodoDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<TodoGroupEntity> Handle(UpdateTodoGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.TodoGroups
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new EntityNotFoundException(nameof(TodoGroupEntity), request.Id);

        entity.Name = request.Name;
        entity.Version = Guid.NewGuid();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
