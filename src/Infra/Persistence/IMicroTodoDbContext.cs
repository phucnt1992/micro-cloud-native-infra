using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Entities;

namespace MicroTodo.Infra.Persistence;

public interface IMicroTodoDbContext
{
    DbSet<TodoEntity> TodoList { get; set; }
    DbSet<TodoGroupEntity> TodoGroups { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
