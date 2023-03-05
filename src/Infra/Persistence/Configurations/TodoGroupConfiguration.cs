using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MicroTodo.Domain.Entities;

namespace MicroTodo.Infra.Persistence.Configurations;

public class TodoGroupConfiguration : IEntityTypeConfiguration<TodoGroup>
{
    public void Configure(EntityTypeBuilder<TodoGroup> builder)
    {
        builder.ToTable("todo_groups");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(255);

        builder.HasMany(x => x.TodoList)
            .WithOne(x => x.BelongToGroup)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
