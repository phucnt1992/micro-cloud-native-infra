using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Commands;
public class DeleteTodoByIdCommandHandler : IRequestHandler<DeleteTodoByIdCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteTodoByIdCommandHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteTodoByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.TodoList
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new EntityNotFoundException(nameof(TodoEntity), request.Id);

        _dbContext.TodoList.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
