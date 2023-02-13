using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Queries;

public class GetTodoGroupDetailByIdQueryHandler : IRequestHandler<GetTodoGroupDetailByIdQuery, TodoGroupEntity>
{
    private readonly IMicroTodoDbContext _dbContext;

    public GetTodoGroupDetailByIdQueryHandler(IMicroTodoDbContext dbContext)
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
