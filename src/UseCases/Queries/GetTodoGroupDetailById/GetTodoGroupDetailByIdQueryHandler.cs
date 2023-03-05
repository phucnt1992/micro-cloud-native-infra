using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Queries;

public class GetTodoGroupDetailByIdQueryHandler : IRequestHandler<GetTodoGroupDetailByIdQuery, TodoGroup>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoGroupDetailByIdQueryHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<TodoGroup> Handle(GetTodoGroupDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var todoGroup = await _dbContext.TodoGroups
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        EntityNotFoundException.ThrowIfNull(todoGroup, request.Id);

        return todoGroup;
    }
}
