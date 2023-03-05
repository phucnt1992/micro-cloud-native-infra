namespace MicroTodo.UseCases.Queries;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

public class GetTodoListByGroupIdQueryHandler : IRequestHandler<GetTodoListByGroupIdQuery, IEnumerable<TodoItem>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoListByGroupIdQueryHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TodoItem>> Handle(GetTodoListByGroupIdQuery request, CancellationToken cancellationToken)
    {
        EntityNotFoundException.ThrowIfFalse<TodoGroup>(
            await _dbContext.TodoGroups
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.GroupId, cancellationToken),
            request.GroupId);

        return await _dbContext.TodoList
            .AsNoTracking()
            .Where(x => x.GroupId == request.GroupId)
            .ToListAsync(cancellationToken);
    }
}
