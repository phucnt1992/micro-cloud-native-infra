using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Commands;

public class UpdateTodoGroupCommandHandler : IRequestHandler<UpdateTodoGroupCommand, TodoGroup>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateTodoGroupCommandHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<TodoGroup> Handle(UpdateTodoGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.TodoGroups
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        EntityNotFoundException.ThrowIfNull(entity, request.Id);

        entity.Name = request.Name;

        entity.ModifiedOn = DateTime.UtcNow;
        entity.Version = Guid.NewGuid();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
