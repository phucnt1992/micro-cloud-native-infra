using MicroTodo.Infra.Persistence;

namespace MicroTodo.UseCases.TodoGroups.Commands;

public class CreateTodoGroupCommandHandler : IRequestHandler<CreateTodoGroupCommand, long>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateTodoGroupCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> Handle(CreateTodoGroupCommand request, CancellationToken cancellationToken)
    {
        var todoGroup = new TodoGroup
        {
            Name = request.Name,
        };

        _dbContext.TodoGroups.Add(todoGroup);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return todoGroup.Id.GetValueOrDefault();
    }
}
