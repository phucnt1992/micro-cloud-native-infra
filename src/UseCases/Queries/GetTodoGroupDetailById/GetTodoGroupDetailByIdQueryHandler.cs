using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Queries;

public class GetTodoGroupDetailByIdQueryHandler : IRequestHandler<GetTodoGroupDetailByIdQuery, TodoGroupEntity>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoGroupDetailByIdQueryHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<TodoGroupEntity> Handle(GetTodoGroupDetailByIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.TodoGroups
            .AsNoTracking()
            .SingleAsync(x => x.Id == request.Id, cancellationToken);
    }
}
