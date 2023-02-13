using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Queries;

public class GetTodoDetailByIdQueryHandler : IRequestHandler<GetTodoDetailByIdQuery, TodoEntity>
{
    private readonly IMicroTodoDbContext _dbContext;

    public GetTodoDetailByIdQueryHandler(IMicroTodoDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<TodoEntity> Handle(GetTodoDetailByIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.TodoList
            .AsNoTracking()
            .SingleAsync(x => x.Id == request.Id, cancellationToken);
    }
}
