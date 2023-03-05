namespace MicroTodo.Domain.Entities;

public class TodoGroup : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<TodoItem> TodoList { get; set; } = new List<TodoItem>();
}
