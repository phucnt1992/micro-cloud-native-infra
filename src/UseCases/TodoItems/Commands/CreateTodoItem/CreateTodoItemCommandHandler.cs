namespace MicroTodo.UseCases.TodoItems.Commands;

using MicroTodo.Infra.Persistence;

class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, long>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateTodoItemCommandHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<long> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todo = new TodoItem
        {
            Title = request.Title,
            State = request.State,
            DueDate = request.DueDate,
            GroupId = request.GroupId,
        };

        _dbContext.TodoItems.Add(todo);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return todo.Id.GetValueOrDefault();
    }
}
