using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.Commands;

class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, long>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateTodoCommandHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<long> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = new TodoEntity
        {
            Title = request.Title,
            State = request.State,
            DueDate = request.DueDate,
            GroupId = request.GroupId,
        };

        _dbContext.TodoList.Add(todo);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return todo.Id.GetValueOrDefault();
    }
}
