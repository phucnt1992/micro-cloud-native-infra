namespace MicroTodo.UseCases.TodoGroups.Queries;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;
public class GetTodoGroupsQueryHandler : IRequestHandler<GetTodoGroupsQuery, IEnumerable<TodoGroup>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoGroupsQueryHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TodoGroup>> Handle(GetTodoGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.TodoGroups
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
