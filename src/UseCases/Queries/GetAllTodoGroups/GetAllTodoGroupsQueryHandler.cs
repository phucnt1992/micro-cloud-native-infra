namespace MicroTodo.UseCases.Queries;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;
public class GetTodoGroupQueryHandler : IRequestHandler<GetAllTodoGroupsQuery, IEnumerable<TodoGroup>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoGroupQueryHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TodoGroup>> Handle(GetAllTodoGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.TodoGroups
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
