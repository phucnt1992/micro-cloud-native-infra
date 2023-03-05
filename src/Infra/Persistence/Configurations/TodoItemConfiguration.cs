namespace MicroTodo.Infra.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MicroTodo.Domain.Entities;

class TodoItemConfiguration : BaseConfiguration<TodoItem>, IEntityTypeConfiguration<TodoItem>
{
    public override void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("todo");
        builder.Property(x => x.Title)
            .HasColumnName("title")
            .IsRequired();

        builder.Property(x => x.State)
            .HasColumnName("state")
            .IsRequired();

        builder.Property(x => x.DueDate)
            .HasColumnName("due_date");
    }
}
