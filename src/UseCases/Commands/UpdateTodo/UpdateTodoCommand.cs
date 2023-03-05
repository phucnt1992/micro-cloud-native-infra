namespace MicroTodo.UseCases.Commands;

public record UpdateTodoCommand : IRequest<TodoItem>
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TodoItemState State { get; set; } = TodoItemState.NotStarted;
    public DateTime? DueDate { get; set; }
    public long? GroupId { get; set; }
}
