namespace MicroTodo.UseCases.TodoGroups.Queries;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

public class GetTodoGroupByIdQueryHandler : IRequestHandler<GetTodoGroupByIdQuery, TodoGroup>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoGroupByIdQueryHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<TodoGroup> Handle(GetTodoGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var todoGroup = await _dbContext.TodoGroups
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        EntityNotFoundException.ThrowIfNull(todoGroup, request.Id);

        return todoGroup;
    }
}
