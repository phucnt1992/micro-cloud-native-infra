namespace MicroTodo.UseCases.TodoItems.Queries;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

public class GetTodoItemsByGroupIdQueryHandler : IRequestHandler<GetTodoItemsByGroupIdQuery, IEnumerable<TodoItem>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoItemsByGroupIdQueryHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TodoItem>> Handle(GetTodoItemsByGroupIdQuery request, CancellationToken cancellationToken)
    {
        EntityNotFoundException.ThrowIfFalse<TodoGroup>(
            await _dbContext.TodoGroups
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.GroupId, cancellationToken),
            request.GroupId);

        return await _dbContext.TodoItems
            .AsNoTracking()
            .Where(x => x.GroupId == request.GroupId)
            .ToListAsync(cancellationToken);
    }
}
