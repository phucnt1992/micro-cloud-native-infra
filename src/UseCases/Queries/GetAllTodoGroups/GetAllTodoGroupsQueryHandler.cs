namespace MicroTodo.UseCases.Queries;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;
public class GetTodoGroupQueryHandler : IRequestHandler<GetAllTodoGroupsQuery, IEnumerable<TodoGroupEntity>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoGroupQueryHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TodoGroupEntity>> Handle(GetAllTodoGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.TodoGroups
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}