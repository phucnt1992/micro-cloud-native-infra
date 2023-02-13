using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Commands;
public class DeleteTodoGroupByIdCommandHandler : IRequestHandler<DeleteTodoGroupByIdCommand>
{
    private readonly IMicroTodoDbContext _dbContext;

    public DeleteTodoGroupByIdCommandHandler(IMicroTodoDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteTodoGroupByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.TodoGroups
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new EntityNotFoundException(nameof(TodoGroupEntity), request.Id);

        _dbContext.TodoGroups.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
