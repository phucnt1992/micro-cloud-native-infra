using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Queries;

public class GetTodoDetailByIdQueryHandler : IRequestHandler<GetTodoDetailByIdQuery, TodoItem>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoDetailByIdQueryHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<TodoItem> Handle(GetTodoDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var todoGroup = await _dbContext.TodoList
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        EntityNotFoundException.ThrowIfNull(todoGroup, request.Id);

        return todoGroup;
    }
}
