namespace MicroTodo.UseCases.TodoItems.Commands;

public record UpdateTodoItemCommand : IRequest<TodoItem>
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TodoItemState State { get; set; } = TodoItemState.NotStarted;
    public DateTime? DueDate { get; set; }
    public long? GroupId { get; set; }
}
