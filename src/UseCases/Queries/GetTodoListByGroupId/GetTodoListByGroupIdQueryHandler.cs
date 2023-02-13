using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Queries;

public class GetTodoListByGroupIdQueryHandler : IRequestHandler<GetTodoListByGroupIdQuery, IEnumerable<TodoEntity>>
{
    private readonly IMicroTodoDbContext _dbContext;

    public GetTodoListByGroupIdQueryHandler(IMicroTodoDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TodoEntity>> Handle(GetTodoListByGroupIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.TodoList
            .AsNoTracking()
            .Where(x => x.GroupId == request.GroupId)
            .ToListAsync(cancellationToken);
    }
}
