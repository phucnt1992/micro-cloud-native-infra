using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using MicroTodo.Domain.Entities;

namespace MicroTodo.Infra.Persistence;

public interface IApplicationDbContext
{
    DatabaseFacade Database { get; }
    DbSet<TodoItem> TodoItems { get; set; }
    DbSet<TodoGroup> TodoGroups { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
