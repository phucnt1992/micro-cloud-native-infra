namespace MicroTodo.UseCases.TodoItems.Commands;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

public class DeleteTodoItemByIdCommandHandler : IRequestHandler<DeleteTodoItemByIdCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteTodoItemByIdCommandHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteTodoItemByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.TodoItems
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        EntityNotFoundException.ThrowIfNull(entity, request.Id);

        _dbContext.TodoItems.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
