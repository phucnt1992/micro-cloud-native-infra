using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Commands;
public class DeleteTodoGroupByIdCommandHandler : IRequestHandler<DeleteTodoGroupByIdCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteTodoGroupByIdCommandHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteTodoGroupByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.TodoGroups
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        EntityNotFoundException.ThrowIfNull(entity, request.Id);

        _dbContext.TodoGroups.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
