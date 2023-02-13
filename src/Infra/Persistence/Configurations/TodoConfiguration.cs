namespace MicroTodo.Infra.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MicroTodo.Domain.Entities;

class TodoConfiguration : BaseConfiguration<TodoEntity>, IEntityTypeConfiguration<TodoEntity>
{
    public override void Configure(EntityTypeBuilder<TodoEntity> builder)
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
