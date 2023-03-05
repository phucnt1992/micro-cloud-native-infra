namespace MicroTodo.Infra.Persistence;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Domain.Entities;
using MicroTodo.Infra.Persistence.Configurations;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<TodoItem> TodoList { get; set; } = null!;
    public DbSet<TodoGroup> TodoGroups { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoConfiguration());
        modelBuilder.ApplyConfiguration(new TodoGroupConfiguration());
    }
}
