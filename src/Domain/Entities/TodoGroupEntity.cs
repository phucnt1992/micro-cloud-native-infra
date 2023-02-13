namespace MicroTodo.Domain.Entities;

public class TodoGroupEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<TodoEntity> TodoList { get; set; } = new List<TodoEntity>();
}
