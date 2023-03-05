namespace MicroTodo.UseCases.TodoItems.Queries;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Exceptions;
using MicroTodo.Infra.Persistence;

public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItem>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoItemByIdQueryHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<TodoItem> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        var todoGroup = await _dbContext.TodoItems
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        EntityNotFoundException.ThrowIfNull(todoGroup, request.Id);

        return todoGroup;
    }
}
