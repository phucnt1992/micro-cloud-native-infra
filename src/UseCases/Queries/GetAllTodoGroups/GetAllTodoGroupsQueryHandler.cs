namespace MicroTodo.UseCases.Queries;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;
public class GetTodoGroupQueryHandler : IRequestHandler<GetAllTodoGroupsQuery, IEnumerable<TodoGroupEntity>>
{
    private readonly IMicroTodoDbContext _dbContext;

    public GetTodoGroupQueryHandler(IMicroTodoDbContext dbContext)
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
