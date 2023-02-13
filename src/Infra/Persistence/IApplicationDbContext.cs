using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using MicroTodo.Domain.Entities;

namespace MicroTodo.Infra.Persistence;

public interface IApplicationDbContext
{
    DatabaseFacade Database { get; }
    DbSet<TodoEntity> TodoList { get; set; }
    DbSet<TodoGroupEntity> TodoGroups { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
