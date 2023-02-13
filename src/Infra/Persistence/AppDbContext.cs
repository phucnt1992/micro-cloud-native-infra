namespace MicroTodo.Infra.Persistence;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Entities;
using MicroTodo.Infra.Persistence.Configurations;

public class AppDbContext : DbContext, IMicroTodoDbContext
{
    public DbSet<TodoEntity> TodoList { get; set; } = null!;
    public DbSet<TodoGroupEntity> TodoGroups { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoConfiguration());
        modelBuilder.ApplyConfiguration(new TodoGroupConfiguration());
    }
}
