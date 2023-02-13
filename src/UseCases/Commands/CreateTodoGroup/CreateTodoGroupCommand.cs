namespace MicroTodo.UseCases.Commands;

public record CreateTodoGroupCommand : IRequest<long>
{
    public string Name { get; set; } = string.Empty;
}
